using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndexer : MonoBehaviour
{
    public WeaponsList weaponsList;

    static public WeaponsList Weapons { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Weapons = weaponsList;
    }
}
