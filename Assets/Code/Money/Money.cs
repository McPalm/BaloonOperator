using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value = 10;
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioPool.PlaySound(transform.position, clip, 1f);
        FindObjectOfType<Wallet>().AddMoney(value);
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
