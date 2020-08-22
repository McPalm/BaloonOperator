using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Wallet>().AddMoney(value);
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
