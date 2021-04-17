using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public int damageTaken;
    public WeaponProperties WeaponProperties;        

    public Weapon() { }

    public Weapon(WeaponProperties weaponProperties, int damageTaken = 0)
    {
        WeaponProperties = weaponProperties;
        this.damageTaken = damageTaken;
    }

    public Weapon(Weapon prefab)
    {
        WeaponProperties = prefab.WeaponProperties;
    }


    public bool Broken => damageTaken > WeaponProperties.durability;

}
