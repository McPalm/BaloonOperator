using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    public int damage;
    public GameObject source;
    public bool authorative;
    public bool reject = false;
    public int terrainDamage = 0;
    public float stunDuration = .5f;

    public DamageData() { }

    public DamageData(DamageProperties props)
    {
        damage = props.damage;
        terrainDamage = props.terrainDamage;
        stunDuration = props.stun;
    }

    public DamageProperties GetProps()
    {
        return new DamageProperties()
        {
            damage = damage,
            terrainDamage = terrainDamage,
            stun = stunDuration,
        };
    }
}
