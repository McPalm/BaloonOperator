using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [Scene]
    public string startLevel;

    public SceneLoader SceneLoader { get; set; }
    public GameObject Player { get; set; }
    public List<GameObject> AllPlayers { get; set; }

    float resetCooldown = 0f;

    private void Awake()
    {
        AllPlayers = new List<GameObject>();
    }

    // Start is called before the first frame update
    [Server]
    void Start()
    {
        SceneLoader = GetComponent<SceneLoader>();
        SceneLoader.LoadScene(startLevel);
        StartCoroutine(StateMachine());
    }

    bool IsGameOver()
    {
        if (AllPlayers.Count > 0)
        {
            foreach (GameObject go in AllPlayers)
            {
                if (go.GetComponent<Health>().CurrentHealth > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator StateMachine()
    {
        yield return new WaitForSeconds(1f);
    playState:
        for (; ; )
        {
            yield return null;
            if (IsGameOver())
                goto gameOverState;
        }
    gameOverState:
        yield return new WaitForSeconds(3f);
        TriggerLoss();
        goto playState;
    }

    [Server]
    public void TriggerLoss()
    {
        Debug.Log("You LOSE!");
        ResetScene();
    }

    [Server]
    public void TriggerWin()
    {
        if (isServer)
        {
            Debug.Log("You WIN!");
            ResetScene();
        }
    }

    [Server]
    public void ResetScene()
    {
        if(resetCooldown > Time.time)
            throw new System.Exception("Your reloading the stage to quickly");
        resetCooldown = Time.time + 1f;
        SceneLoader.ReloadScene();
        foreach (GameObject go in AllPlayers)
        {
            go.GetComponent<Health>().Heal(new DamageData()
            {
                damage = 999,
                source = gameObject,
                authorative = true,
            });
        }
        ///Probably add reset health to character since they aren't a part of the scene.
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }

    public void RegisterAllPlayers(GameObject player)
    {
        AllPlayers.Add(player);
    }

}
