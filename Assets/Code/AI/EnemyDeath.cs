using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public bool flipY = true;

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
        if(flipY)
            GetComponentInChildren<SpriteRenderer>().flipY = true;
        ContactDamage contactDamage = GetComponentInChildren<ContactDamage>();
        if(contactDamage)
            contactDamage.enabled = false;
    }
}
