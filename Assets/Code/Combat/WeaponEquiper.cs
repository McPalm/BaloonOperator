using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquiper : MonoBehaviour
{
    [SerializeField] WeaponProperties equipped;

    public SpriteRenderer spriteRenderer;
    public Transform swipeTrail;
    public ContactDamage ContactDamage;
    public Animator Animator;
    public PolygonCollider2D Collider;
    public ProjectileSpawner ProjectileSpawner;
    public GameObject Bowstring;

    public WeaponProperties Equipped => equipped;

    // Start is called before the first frame update
    void Start()
    {
        Equip(equipped);
    }


    public void Equip(WeaponProperties weapon)
    {
        equipped = weapon;
        spriteRenderer.sprite = weapon.sprite;
        swipeTrail.localPosition = new Vector3(weapon.lenght, 0f);
        ContactDamage.damagePropeties = weapon.damageProperties;

        Animator.SetFloat("AttackSpeed", weapon.attackSpeed);
        Animator.SetBool("Hammer", weapon.hammer);
        Animator.SetBool("Spear", weapon.spear);
        Animator.SetBool("Bow", weapon.bow);
        Animator.SetBool("OneHanded", weapon.oneHander);
        Animator.SetBool("Stabbing", weapon.stabbing);
        Animator.SetBool("Slashing", weapon.slashing);
        List<Vector2> points = new List<Vector2>();
        weapon.sprite.GetPhysicsShape(0, points);
        Collider.SetPath(0, points);
        ProjectileSpawner.damage = weapon.damageProperties;
        Bowstring.SetActive(weapon.bow);
    }

    
}
