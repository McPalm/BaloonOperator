using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponsList", menuName = "WeaponsList", order = 123451)]
public class WeaponsList : ScriptableObject
{
    public WeaponProperties[] WeaponProperties;

    public int IndexFor(WeaponProperties p)
    {
        for(int i = 0; i < WeaponProperties.Length; i++)
        {
            if (WeaponProperties[i] == p)
                return i;
        }
        throw new System.Exception($"Weapon {p} missing in Weapons List!");
    }

    public WeaponProperties WeaponFor(int index)
    {
        return WeaponProperties[index];
    }
    
}
