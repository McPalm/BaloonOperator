using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToss : MonoBehaviour
{
    public Projectile prefab;

    public event System.Action<Weapon> OnThrow;

    public WeaponEquiper WeaponEquiper;

    public float Y;

    private void OnEnable()
    {
        var projectile = ProjectileFactory();
        projectile.transform.right = new Vector3(transform.Forward() * 1.5f, Y, 0f);

        // projectile.transform.SetForward(transform.Forward());
    }

    Projectile ProjectileFactory()
    {
        var clone = Instantiate(prefab, transform.position, Quaternion.identity);
        var weapon = WeaponEquiper.Equipped;
        var properties = weapon.WeaponProperties;
        clone.GetComponent<SpriteRenderer>().sprite = properties.sprite;
        clone.GetComponent<ContactDamage>().damagePropeties = properties.throwDamage;
        clone.GetComponent<ContactDamage>().source = gameObject;
        clone.speed = new Vector2(25, 5f);
        clone.gravity = 36f;
        clone.airFriction = .99f;

        clone.GetComponent<ContactDamage>().OnHit += (h) =>
        {
            weapon.damageTaken += 3;
            if (weapon.Broken)
            {
                Destroy(clone.gameObject);
                AudioPool.PlaySound(clone.transform.position, WeaponEquiper.BreakSound, .75f);
            }
        };
        clone.GetComponent<DestroyOnTouch>().OnTouch += () =>
        {
            if (!weapon.Broken)
                FindObjectOfType<ItemManager>().Spawn(properties, clone.transform.position, weapon.damageTaken);
        };


        OnThrow?.Invoke(weapon);
        return clone;
    }
}
