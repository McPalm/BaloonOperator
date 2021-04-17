using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : NetworkBehaviour
{
    [SyncVar]
    int weaponIndex;
    public WeaponProperties Weapon;
    public SpriteRenderer SpriteRenderer;
    [SyncVar]
    public int durabilityLoss = 0;
         
    // Start is called before the first frame update
    void Start()
    {
        if(isServer)
        {
            weaponIndex = ItemIndexer.Weapons.IndexFor(Weapon);
            RpcSet(weaponIndex);
        }
        else
        {
            Weapon = ItemIndexer.Weapons.WeaponFor(weaponIndex);
            SpriteRenderer.sprite = Weapon.sprite;
        }
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcSet(int index)
    {
        weaponIndex = index;
        Weapon = ItemIndexer.Weapons.WeaponFor(index);
        SpriteRenderer.sprite = Weapon.sprite;
    }
}
