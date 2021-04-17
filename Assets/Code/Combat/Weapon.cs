using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{

    public Weapon() { }

    public Weapon(WeaponProperties weaponProperties)
    {
        WeaponProperties = weaponProperties;
    }

    public WeaponProperties WeaponProperties;   
}
