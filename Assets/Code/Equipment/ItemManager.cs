﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : NetworkBehaviour
{
    public WeaponPickup WeaponPickupPrefab;
    
    public WeaponPickup Spawn(WeaponProperties weapon, Vector2 position)
    {
        var obj = Instantiate(WeaponPickupPrefab, position, Quaternion.identity, transform);
        obj.Weapon = weapon;
        NetworkServer.Spawn(obj.gameObject);
        return obj;
    }
}