using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HydraCoordinator : NetworkBehaviour
{

    public float enrageTimer = 300f;

    public HydraAI[] heads;

    bool enraged = false;

    void Start()
    {
        heads.ForEach(h => h.gameObject.SetActive(false));
        if(isServer)
        {
            for(int i = 0; i < heads.Length; i++)
            {
                int capture = i;
                heads[i].GetComponent<Health>().OnZeroHealth += () => HydraCoordinator_OnZeroHealth(capture);
            }
        }
    }

    public int headKills = 0;

    private void HydraCoordinator_OnZeroHealth(int index)
    {
        Debug.Log("Is dead!");
        var head = heads[index];
        if (head.isActiveAndEnabled)
        {
            headKills++;
            StartCoroutine(KillHeadRoutine(head));
        }
    }

    IEnumerator KillHeadRoutine(HydraAI head)
    {
        head.gameObject.SetActive(false);
        Debug.Log("inactive");
        head.GetComponent<Health>().Heal(new DamageData()
        {
            damage = 999,
            authorative = true,
        });
        if (headKills < 4)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("wake up two");
            int count = 0;
            for (int i = 0; i < heads.Length; i++)
            {
                if (heads[i].isActiveAndEnabled == false)
                {
                    RpcActivateHead(i);
                    if (++count == 2)
                        break;
                    yield return new WaitForSeconds(.5f);
                }
            }
        }
    }

    public void Activate()
    {
        Debug.Log("Wake up!");
        if (MyNetworkManager.isServer)
        {
            StartCoroutine(MainLoop());
        }
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcActivateHead(int head)
    {
        heads[head].gameObject.SetActive(true);
    }

    IEnumerator MainLoop()
    {
        yield return new WaitForSeconds(.1f);
        RpcActivateHead(1);
        yield return new WaitForSeconds(1f);

        enrageTimer += Time.timeSinceLevelLoad;
        var gameManager = FindObjectOfType<GameManager>();
        for(; ; )
        {
            bool canDo = true;
            foreach(var head in heads)
            {
                if (head.Attacking)
                    canDo = false;
            }
            if(canDo)
            {
                var scramble = heads.Where(h => h.gameObject.active).ToList();
                scramble.Sort((a, b) => Random.value < .5f ? 1 : -1);
                int attack = 0;
                foreach(var head in scramble)
                {
                    head.SetAttack(gameManager.AllPlayers[Random.Range(0, gameManager.AllPlayers.Count)] ,attack++ % 2 + 1);
                    if(!enraged)
                    {
                        yield return new WaitForSeconds(.5f);
                        head.UnAttack();
                    }
                }
                if(enraged)
                {
                    yield return new WaitForSeconds(.5f);
                    scramble.ForEach(h => h.UnAttack());
                }
            }
            if(Time.timeSinceLevelLoad > enrageTimer && !enraged)
            {
                enraged = true;
                heads.ForEach(h => h.enraged = true);
                Debug.Log("Enraged!");
            }

            yield return new WaitForSeconds(Random.value);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
