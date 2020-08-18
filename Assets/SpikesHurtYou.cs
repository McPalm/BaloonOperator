using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesHurtYou : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Spikes!");
        var mobile = collision.GetComponent<Mobile>();
        var health = collision.GetComponent<Health>();
        if (mobile && health)
        {
            if (mobile.VMomentum < -16f)
                health.Hurt(3);
            else if (mobile.VMomentum < -8f)
                health.Hurt(2);
            else if (mobile.VMomentum < -3f)
                health.Hurt(1);

        }
    }
}
