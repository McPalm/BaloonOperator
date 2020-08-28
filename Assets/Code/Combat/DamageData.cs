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
         
}
