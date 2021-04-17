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

    AnimationPhysics AnimationPhysics;

    public Weapon StartWeapon;
    public Weapon[] HeldWeapons;
    public Weapon CurrentWeapon => WeaponEquiper.Equipped;

    public event System.Action<Inventory> OnWeaponChange;

    public void Clear()
    {
        HeldWeapons = new Weapon[5];
        PutInSlot(StartWeapon, 0);
    }

    void Start()
    {
        AnimationPhysics = WeaponEquiper.GetComponent<AnimationPhysics>();
        Clear();
        GetComponent<Health>().OnZeroHealth += Inventory_OnZeroHealth;
    }

    private void Inventory_OnZeroHealth()
    {
        for (int i = 0; i < HeldWeapons.Length; i++)
        {
            if (HeldWeapons[i] != null)
                Drop(i);
        }   
    }

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
                    Pickup(weapon);
                    InputToken.ConsumeInteract();
                }
                var shopItem = inter.GetComponent<ShopPickup>();
                if(shopItem && FindObjectOfType<Wallet>().held >= shopItem.price)
                {
                    Pickup(shopItem);
                    InputToken.ConsumeInteract();
                }
            }
        }
    }

    void Pickup(WeaponPickup item)
    {
        for (int i = 0; i < HeldWeapons.Length; i++)
        {
            if(i == 4 || HeldWeapons[i] == null)
            {
                PutInSlot(new Weapon(item.Weapon) , i);
                if (i == 4)
                    EquipSlot(4);
                Destroy(item.gameObject);
                break;
            }
        }
    }
    void Pickup(ShopPickup item)
    {
        for (int i = 0; i < HeldWeapons.Length; i++)
        {
            if (i == 4 || HeldWeapons[i] == null)
            {
                PutInSlot(new Weapon(item.weaponProperties), i);
                if(i == 4)
                    EquipSlot(4);
                Destroy(item.gameObject);
                break;    
            }
        }
    }
    void Drop(Weapon weapon)
    {
        for (int i = 0; i < HeldWeapons.Length; i++)
        {
            if(HeldWeapons[i] == weapon)
            {
                Drop(i);
                return;
            }
        }
    }
    void Drop(int slot)
    {
        if(HeldWeapons[slot] != null)
        {
            if (HeldWeapons[slot] == WeaponEquiper.Equipped)
                WeaponEquiper.Equip(null);
            CmdDropWeapon(WeaponsList.IndexFor(HeldWeapons[slot].WeaponProperties));
        }
        HeldWeapons[slot] = null;
        OnWeaponChange?.Invoke(this);
    }
    void PutInSlot(Weapon weapon, int slot)
    {
        if (HeldWeapons[slot] != null)
            Drop(HeldWeapons[slot]);
        HeldWeapons[slot] = weapon;
        EquipSlot(slot);
        OnWeaponChange?.Invoke(this);
    }

    public void DropCurrent() => Drop(WeaponEquiper.Equipped);

    [Command(channel = Channels.DefaultReliable)]
    void CmdDropWeapon(int index)
    {
        FindObjectOfType<ItemManager>().Spawn(WeaponsList.WeaponFor(index), transform.position);
    }

    public void EquipSlot(int slot)
    {
        if (slot < HeldWeapons.Length && HeldWeapons[slot] != null && AnimationPhysics.easyCancel)
        {
            // CmdEquip(WeaponsList.IndexFor(HeldWeapons[slot].WeaponProperties));
            WeaponEquiper.Equip(HeldWeapons[slot]);
            if (slot < 4 && HeldWeapons[4] != null)
                Drop(HeldWeapons[4]);
            OnWeaponChange?.Invoke(this);
        }
    }

    [Command(channel = Channels.DefaultReliable)]
    public void CmdSwap(NetworkIdentity target)
    {
        var weapon = target.GetComponent<WeaponPickup>();
        if(weapon && weapon.isActiveAndEnabled)
        {
            // spawn replacement weapon on server
            FindObjectOfType<ItemManager>().Spawn(WeaponEquiper.EquippedProperties, transform.position);

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
 
            FindObjectOfType<ItemManager>().Spawn(WeaponEquiper.EquippedProperties, transform.position);
            shopItem.gameObject.SetActive(false);
            var index = WeaponsList.IndexFor(shopItem.weaponProperties);
            RpcEquip(index, target);
            return;
        }
    }

    [Command(channel = Channels.DefaultReliable)]
    public void CmdEquip(int weaponIndex)
    {
        RpcEquip(weaponIndex, null);
    }

    [ClientRpc(channel=Channels.DefaultReliable)]
    public void RpcEquip(int weaponIndex, NetworkIdentity old)
    {
        // get index of the weapon
        var weapon = new Weapon()
        {
            WeaponProperties = WeaponsList.WeaponProperties[weaponIndex]
        }; // Hack
        // equip it
        WeaponEquiper.Equip(weapon);
        old?.gameObject.SetActive(false);
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
