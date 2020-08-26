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
        GetComponent<Health>().OnChangeTrueHealth += TellyAI_OnChangeTrueHealth;
    }

    private void TellyAI_OnChangeTrueHealth(int hp, int chane)
    {
        if (hp <= 0)
            Mobile.Gravity = 8f;
    }

    public void Movement()
    {
        if(currentTarget == null || IsStunned)
        {
            Mobile.HMomentum = 0f;
            Mobile.VMomentum = IsStunned ? 0f : 2f;
        }
        else
        {
            float sqrDistance = Vector3.SqrMagnitude(currentTarget.transform.position - transform.position);
            float sqrOptimal = optimalRange * optimalRange;
            if (sqrDistance > sqrOptimal + .75f)
            {
                Vector3 direction = Vector3.Normalize(currentTarget.transform.position - transform.position);

                Mobile.HMomentum = direction.x * speed;
                Mobile.VMomentum = direction.y * speed;
            }
            else if (sqrDistance < sqrOptimal - .75f)
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

    private void FixedUpdate()
    {
        Movement();
    }

    protected override IEnumerator AwakeCoroutine()
    {
        yield return null;
        var attack = GetComponent<EnemyAttack>();
    idle:
        currentTarget = null;
        for (; ; )
        {
            currentTarget = FindTarget(targetRange);
            if (currentTarget != null && currentTarget.transform.position.y < transform.position.y)
                goto attack;
            else
                currentTarget = null;
            yield return new WaitForSeconds(Random.value * .2f);
        }
    attack:
        for (; ; )
        {
            if (currentTarget.GetComponent<Health>().CurrentHealth <= 0)
                goto idle;
            {
                attack.SetTarget(currentTarget.GetComponent<NetworkIdentity>());
                attack.Attack();
                yield return new WaitForSeconds(attackInterval + Random.value * .25f);

            }
        }
    }
}
