using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public bool flipY = true;
    public ParticleRef DeathParticles;
    public AudioClip DeathSound;

    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        health.OnZeroHealth += Kill;
    }


    void Kill()
    {
        GetComponent<EnemyController>().enabled = false;
        GetComponent<Mobile>().HMomentum = 0f;
        SpriteRenderer ren = GetComponentInChildren<SpriteRenderer>();
        if (flipY)
            ren.flipY = true;
        ren.sortingLayerName = "Background";
        ren.sortingOrder = -5;
        var flash = ren.GetComponent<SpriteColourFlash>();
        flash.enabled = false;
        GetComponentsInChildren<ContactDamage>().ForEach(cd => cd.enabled = false);
        if (DeathParticles)
            DeathParticles.Play(transform.position);
        if (DeathSound)
            AudioPool.PlaySound(transform.position, DeathSound);
    }
}
