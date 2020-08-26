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

    // Start is called before the first frame update
    void Start()
    {
        Equip(equipped);
    }


    public void Equip(WeaponProperties weapon)
    {
        spriteRenderer.sprite = weapon.sprite;
        swipeTrail.localPosition = new Vector3(weapon.lenght, 0f);
        ContactDamage.damage = weapon.damage;
        Animator.SetFloat("AttackSpeed", weapon.attackSpeed);
        Animator.SetBool("Hammer", weapon.hammer);
        List<Vector2> points = new List<Vector2>();
        weapon.sprite.GetPhysicsShape(0, points);
        Collider.SetPath(0, points);
    }

    
}
