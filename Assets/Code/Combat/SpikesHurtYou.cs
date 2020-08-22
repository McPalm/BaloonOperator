using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesHurtYou : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var mobile = collision.GetComponent<Mobile>();
        var health = collision.GetComponent<Health>();
        var player = collision.GetComponent<PlatformingCharacter>();
        if (mobile && health)
        {
            if(player)
            {
                if (player.Climbing)
                    return;
                if(player.WallSliding)
                return;
            }

            if (mobile.VMomentum < -18f)
                Hurt(health, 3);
            else if (mobile.VMomentum < -9f)
                Hurt(health, 2);
            else if (mobile.VMomentum < -4f)
                Hurt(health, 1);

        }
    }

    void Hurt(Health target, int ammount) => target.Hurt(new DamageData() { damage = ammount, source = gameObject });

}
