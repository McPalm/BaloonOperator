using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPickup : NetworkBehaviour
{
    [SyncVar]
    public int weaponIndex;
    [SyncVar]
    public int price = 500;
    public WeaponProperties weaponProperties;
    public SpriteRenderer Renderer;
    public TextMeshProUGUI priceText;

    void Start()
    {
        if (isServer)
        {
            weaponIndex = ItemIndexer.Weapons.IndexFor(weaponProperties);
            RpcSet(weaponIndex, price);
        }
        else
        {
            weaponProperties = ItemIndexer.Weapons.WeaponFor(weaponIndex);
            Renderer.sprite = weaponProperties.sprite;
        }
        priceText.text = $"{price}";
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcSet(int index, int value)
    {
        weaponIndex = index;
        price = value;
        weaponProperties = ItemIndexer.Weapons.WeaponFor(index);
        Renderer.sprite = weaponProperties.sprite;
        priceText.text = $"{price}";
    }
}
