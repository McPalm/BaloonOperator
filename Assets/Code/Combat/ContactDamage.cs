using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{

    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.GetComponent<Health>();
        if (health)
            health.Hurt(new DamageData()
            {
                damage = damage,
                source = gameObject
            });
    }
    
}
