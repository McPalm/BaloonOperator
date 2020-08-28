using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public GameObject source;
    public int damage = 1;
    public DamageProperties damagePropeties;

    public AudioClip OnHitSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        var health = collision.GetComponent<Health>();
        if (health)
        {

            health.Hurt(new DamageData(damagePropeties)
            {
                source = source ?? gameObject,   
            });
            if(OnHitSound != null)
                AudioPool.PlaySound(clip: OnHitSound, position: transform.position);
        }
    }
    
}
