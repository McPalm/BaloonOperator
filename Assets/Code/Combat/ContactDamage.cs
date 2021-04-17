using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public GameObject source;
    public int damage = 1;
    public DamageProperties damagePropeties;
    public AudioClip OnHitSound;
    public bool hurtDead;

    public event System.Action<Health> OnHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        var health = collision.GetComponent<Health>();
        if (health && (hurtDead || health.CurrentHealth > 0))
        {
            health.Hurt(new DamageData(damagePropeties)
            {
                source = source ?? gameObject,   
            });
            if(OnHitSound != null)
                AudioPool.PlaySound(clip: OnHitSound, position: transform.position);
            OnHit?.Invoke(health);
        }
    }
    
}
