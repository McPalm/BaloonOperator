using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : NetworkBehaviour, IInputReader
{
    public InputToken InputToken { get; set; }

    public LayerMask InteractLayer;
    public WeaponEquiper WeaponEquiper;
    public WeaponsList WeaponsList;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer && InputToken.InteractPressed)
        {
            var inter = GetInteractable();
            if (inter)
            {
                var weapon = inter.GetComponent<WeaponPickup>();
                if (weapon)
                {
                    CmdSwap(weapon.GetComponent<NetworkIdentity>());
                    InputToken.ConsumeInteract();
                }
                var shopItem = inter.GetComponent<ShopPickup>();
                if(shopItem && FindObjectOfType<Wallet>().held >= shopItem.price)
                {
                    CmdSwap(shopItem.GetComponent<NetworkIdentity>());
                    InputToken.ConsumeInteract();
                }
            }
        }
    }

    [Command(channel = Channels.DefaultReliable)]
    public void CmdSwap(NetworkIdentity target)
    {
        var weapon = target.GetComponent<WeaponPickup>();
        if(weapon && weapon.isActiveAndEnabled)
        {
            // spawn replacement weapon on server
            FindObjectOfType<ItemManager>().Spawn(WeaponEquiper.Equipped, transform.position);

            // disable old weapon so it cannot be picked up again
            weapon.gameObject.SetActive(false);

            // equip the new weapon
            var index = WeaponsList.IndexFor(weapon.Weapon);
            RpcEquip(index, target);
            return;
        }
        var shopItem = target.GetComponent<ShopPickup>();
        if(shopItem && shopItem.isActiveAndEnabled && FindObjectOfType<Wallet>().held >= shopItem.price)
        {
            // spend the cash
            FindObjectOfType<Wallet>().SpendMoney(shopItem.price);
 
            FindObjectOfType<ItemManager>().Spawn(WeaponEquiper.Equipped, transform.position);
            shopItem.gameObject.SetActive(false);
            var index = WeaponsList.IndexFor(shopItem.weaponProperties);
            RpcEquip(index, target);
            return;
        }
    }

    [ClientRpc(channel=Channels.DefaultReliable)]
    public void RpcEquip(int weaponIndex, NetworkIdentity old)
    {
        // get index of the weapon
        var weapon = WeaponsList.WeaponProperties[weaponIndex];
        // equip it
        WeaponEquiper.Equip(weapon);
        old.gameObject.SetActive(false);
    }

    GameObject GetInteractable()
    {
        var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, Vector2.zero, 0f, InteractLayer);
        if (hit)
            return hit.transform.gameObject;
        else
            return null;
    }
}
