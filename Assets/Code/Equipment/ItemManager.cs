using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LootTable;

public class ItemManager : NetworkBehaviour
{
    static public ItemManager Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
    }

    public WeaponPickup WeaponPickupPrefab;
    public ShopPickup ShopPickupPrefab;

    public GameObject Spawn(ILoot loot, Vector2 position)
    {
       
        if(loot is WeaponProperties)
        {
            var weapon = Spawn((WeaponProperties)loot, position);
            return weapon.gameObject;
        }
        Debug.Log("No support for type");
        return null;
    }

    public WeaponPickup Spawn(WeaponProperties weapon, Vector2 position)
    {
        var obj = Instantiate(WeaponPickupPrefab, position, Quaternion.identity, transform);
        obj.Weapon = weapon;
        NetworkServer.Spawn(obj.gameObject);
        return obj;
    }

    public GameObject SpawnShopItem(WeaponProperties item, Vector2 position, int price)
    {
        var obj = Instantiate(ShopPickupPrefab, position, Quaternion.identity, transform);
        obj.weaponProperties = item;
        obj.price = price;
        NetworkServer.Spawn(obj.gameObject);
        return obj.gameObject;
    }
}
