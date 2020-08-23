using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        health.OnChangeTrueHealth += Health_OnChangeTrueHealth;
    }

    private void Health_OnChangeTrueHealth(int current, int change)
    {
        if (current <= 0)
            Kill();
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }
}
