using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent<Collider2D> OnTrigger;
    public bool onceOnly = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger?.Invoke(collision);
        if (onceOnly)
            gameObject.SetActive(false);
    }
}
