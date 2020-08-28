using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathHasConsequences : MonoBehaviour
{
    public List<MonoBehaviour> DeadScripts;
    public List<MonoBehaviour> AliveScripts;

    public UnityEvent OnDeath;
    public UnityEvent OnRessurect;

    bool dead;

    public Health Health;


    void Start()
    {
        if (Health == null)
            Health = GetComponent<Health>();
        Health.OnChangeTrueHealth += Health_OnChangeTrueHealth;
    }

    private void Health_OnChangeTrueHealth(int hp, DamageProperties props)
    {
        if(hp <= 0 != dead)
        {
            dead = hp <= 0;
            if(dead)
            {
                foreach (var dis in DeadScripts)
                    dis.enabled = true;
                foreach (var dis in AliveScripts)
                    dis.enabled = false;
                OnDeath.Invoke();
            }
            else
            {
                foreach (var dis in AliveScripts)
                    dis.enabled = true;
                foreach (var dis in DeadScripts)
                    dis.enabled = false;
                OnRessurect.Invoke();
            }
        }
    }
}
