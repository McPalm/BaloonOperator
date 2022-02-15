using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    public int damageResistance = 1;
    public UnityEvent OnDestroy;

    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        health.OnChangeTrueHealth += Health_OnChangeTrueHealth;
        health.ApplyDefence += Health_ApplyDefence;
    }

    private void Health_ApplyDefence(DamageData obj)
    {
        if (obj.terrainDamage < damageResistance)
            obj.damage = 0;
    }

    private void Health_OnChangeTrueHealth(int current, DamageProperties props)
    {
        if (current <= 0)
            Kill();
    }

    private void Kill()
    {
        gameObject.SetActive(false);
        OnDestroy.Invoke();
    }
}
