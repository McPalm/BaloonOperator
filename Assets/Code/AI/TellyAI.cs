using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Searches out enemies from below it, and flies right towards them and stops at an optimal firing range.
// Tries to keep that optimal firing range, could be quite annoying actually, don't make it too fast.
// Doesn't pass walls.
public class TellyAI : EnemyController
{
    public float speed = 2.5f;
    public float targetRange = 10f;
    public float optimalRange = 5f;
    public float attackInterval = 3f;

    PlatformingCharacter currentTarget;

    public override void InitAI()
    {
        Mobile.Gravity = -1;
        StartCoroutine(SearchForTarget());
    }

    IEnumerator SearchForTarget()
    {
        var attack = GetComponent<EnemyAttack>();
        while (true)
        {
            while(!enabled)
            {
                yield return new WaitForSeconds(1f);
            }
            if (currentTarget == null)
            {
                currentTarget = FindTarget(targetRange);
                if (currentTarget!= null && currentTarget.transform.position.y > transform.position.y)
                {
                    currentTarget = null;
                }
            }
            else if(currentTarget.GetComponent<Health>().CurrentHealth <= 0)
            {
                currentTarget = null;
            }
            else
            {
                attack.SetTarget(currentTarget.GetComponent<NetworkIdentity>());
                attack.Attack();
                yield return new WaitForSeconds(attackInterval);
            }
            yield return new WaitForSeconds(Random.value);
        }
    }

    public override void Enemybehaviour()
    {
        if (IsStunned)
        {
            Mobile.HMomentum = 0f;
            return;
        }
        if (currentTarget != null)
        {
            Mobile.Gravity = 0;
            if (Vector3.Distance(currentTarget.transform.position, transform.position) > optimalRange + 0.1f)
            {
                Vector3 direction = Vector3.Normalize(currentTarget.transform.position - transform.position);

                Mobile.HMomentum = direction.x * speed;
                Mobile.VMomentum = direction.y * speed;
            }
            else if (Vector3.Distance(currentTarget.transform.position, transform.position) < optimalRange - 0.1f)
            {
                Vector3 direction = Vector3.Normalize(currentTarget.transform.position - transform.position);

                Mobile.HMomentum = direction.x * -speed;
                Mobile.VMomentum = direction.y * -speed;
            }
            else 
            {
                Mobile.HMomentum = 0;
                Mobile.VMomentum = 0;
            }
        }
    }
}
