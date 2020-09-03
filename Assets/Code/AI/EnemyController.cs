using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class EnemyController : MonoBehaviour
{
    protected Mobile Mobile { get; set; }
    Health Health { get; set; }

    float stunTime;
    protected bool IsStunned => Time.timeSinceLevelLoad < stunTime;

    // Start is called before the first frame update
    protected void Start()
    {
        InitEnemyController();
    }

    public void InitEnemyController()
    {
        Mobile = GetComponent<Mobile>();
        Health = GetComponent<Health>();
        if (Health)
        {
            Health.OnHurt += Health_OnHurt;
            Health.OnChangeTrueHealth += Health_OnChangeTrueHealth;
        }
        InitAI();
    }

    Coroutine ActiveAwakeCoroutine;

    private void OnEnable()
    {
        if(MyNetworkManager.isServer || GetComponent<Mirror.NetworkIdentity>() == null)
            ActiveAwakeCoroutine = StartCoroutine(AwakeCoroutine());
    }

    private void OnDisable()
    {
        if(ActiveAwakeCoroutine != null)
            StopCoroutine(ActiveAwakeCoroutine);
    }

    // put AI loop in here
    abstract protected IEnumerator AwakeCoroutine();
    abstract public void InitAI();

    private void Health_OnChangeTrueHealth(int hp, DamageProperties props)
    {
        if(props.damage > 0)
            stunTime = Mathf.Max(Time.timeSinceLevelLoad + props.stun, stunTime);
    }

    private void Health_OnHurt(DamageData obj)
    {
        stunTime = Mathf.Max(Time.timeSinceLevelLoad + obj.stunDuration, stunTime);
    }

    // Note that this can and will return null.
    protected PlatformingCharacter FindTarget(float limit = 0)
    {
        PlatformingCharacter[] characters = FindObjectsOfType<PlatformingCharacter>();
        characters = characters.Where(c => c.GetComponent<Health>().CurrentHealth > 0).ToArray<PlatformingCharacter>();
        float min;
        
        PlatformingCharacter target = null;
        if (limit == 0)
        {
            min = Vector3.Distance(characters[0].transform.position, transform.position);
            target= characters[0];
        }
        else
        {
            min = limit;
        }
        foreach (PlatformingCharacter character in characters)
        {
            if(Vector3.Distance(character.transform.position, transform.position) < min)
            {
                min = Vector3.Distance(character.transform.position, transform.position);
                target = character;
            }
        }
        return target;
    }
}
