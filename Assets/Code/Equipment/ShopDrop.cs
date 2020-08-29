using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDrop : MonoBehaviour
{
    public List<ShopDropPair> Inventory;

    void Start()
    {
        if(MyNetworkManager.isServer)
        {
            var drop = Inventory[Random.Range(0, Inventory.Count)];
            FindObjectOfType<ItemManager>().SpawnShopItem(drop.item, transform.position, drop.price);
        }
        gameObject.SetActive(false);
    }

    [System.Serializable]
    public struct ShopDropPair
    {
        public WeaponProperties item;
        public int price;
    }
}
