using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquiper : MonoBehaviour
{
    Weapon equipped;

    public SpriteRenderer spriteRenderer;
    public Transform swipeTrail;
    public ContactDamage ContactDamage;
    public Animator Animator;
    public PolygonCollider2D Collider;
    public ProjectileSpawner ProjectileSpawner;
    public GameObject Bowstring;

    public WeaponProperties EquippedProperties => equipped.WeaponProperties;
    public Weapon Equipped => equipped;

    // Start is called before the first frame update
    void Start()
    {
        Equip(equipped);
    }


    public void Equip(Weapon weapon)
    {
        equipped = weapon;
        Animator.SetBool("HasWeapon", equipped != null);
        if (weapon == null)
        {
            spriteRenderer.sprite = null;
            return;
        }
        var props = weapon.WeaponProperties;
        spriteRenderer.sprite = props.sprite;
        swipeTrail.localPosition = new Vector3(props.lenght, 0f);
        ContactDamage.damagePropeties = props.damageProperties;

        Animator.SetTrigger("Equip");
        Animator.SetFloat("AttackSpeed", props.attackSpeed);
        Animator.SetBool("Hammer", props.hammer);
        Animator.SetBool("Spear", props.spear);
        Animator.SetBool("Bow", props.bow);
        Animator.SetBool("OneHanded", props.oneHander);
        Animator.SetBool("Stabbing", props.stabbing);
        Animator.SetBool("Slashing", props.slashing);
        List<Vector2> points = new List<Vector2>();
        props.sprite.GetPhysicsShape(0, points);
        Collider.SetPath(0, points);
        ProjectileSpawner.damage = props.damageProperties;
        Bowstring.SetActive(props.bow);
        ProjectileSpawner.gameObject.SetActive(props.bow);
    }

    
}
