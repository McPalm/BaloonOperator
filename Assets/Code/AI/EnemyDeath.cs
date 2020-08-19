using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
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
        GetComponentInChildren<SpriteRenderer>().flipY = true;
        GetComponentInChildren<ContactDamage>().enabled = false;
    }
}
