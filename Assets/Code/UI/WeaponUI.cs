using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    Inventory Inventory;

    public Image[] images;
    public Image FrameOfEquipped;

    void Setup(Inventory target)
    {
        Inventory = target;
        target.OnWeaponChange += Target_OnWeaponChange;
        Target_OnWeaponChange(Inventory);
        target.WeaponEquiper.OnDurabilityChange += WeaponEquiper_OnDurabilityChange;
    }

    private void WeaponEquiper_OnDurabilityChange(Weapon obj)
    {
        for (int i = 0; i < 5; i++)
        {
            if(obj == Inventory.HeldWeapons[i])
            {
                DrawDurability(i);
            }
        }
    }

    IEnumerator Start()
    {
        Inventory = null;
        while(Inventory == null)
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm?.Player != null)
            {
                Inventory = gm.Player.GetComponent<Inventory>();
            }
            yield return null;
        }
        Setup(Inventory);
    }

    private void Target_OnWeaponChange(Inventory obj)
    {
        FrameOfEquipped.enabled = false;
        for (int i = 0; i < 5; i++)
        {
            var held = obj.HeldWeapons[i];
            
            images[i].gameObject.SetActive(held != null);
            images[i].sprite = obj?.HeldWeapons[i]?.WeaponProperties.sprite;
            DrawDurability(i);

            if (held != null && held == obj.CurrentWeapon)
            {
                FrameOfEquipped.transform.position = images[i].transform.position;
                FrameOfEquipped.enabled = true;
            }
        }
    }

    void DrawDurability(int i)
    {
        var slide = images[i].GetComponentInChildren<Slider>();
        var weapon = Inventory.HeldWeapons[i];
        if(weapon != null)
        {
            slide.maxValue = weapon.WeaponProperties.durability;
            slide.value = slide.maxValue - weapon.damageTaken;
        }
    }
}
