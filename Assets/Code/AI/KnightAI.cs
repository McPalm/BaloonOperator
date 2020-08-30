using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : EnemyController
{
    public float aggoRadius = 6f;
    public float attackRange = 3f;
    public float attackRecoverTime = 1f;
    public float movementSpeed = 2f;

    public EnemyAttack EnemyAttack;

    public override void InitAI()
    {
        
    }

    protected override IEnumerator AwakeCoroutine()
    {
        yield return new WaitForSeconds(1f);
        PlatformingCharacter target = null;

    lookForPlayer:
        target = FindTarget(aggoRadius);
        if (target)
            goto active;
        goto wait;

    active:
        float distance = Vector2.SqrMagnitude(target.transform.position - transform.position);
        if (distance < 1f)
            goto attack;
        if (Random.value < .3f)
            goto approach;
        if (distance < attackRange * attackRange)
            goto attack;
        else
            goto approach;

    attack:
        LookAtTarget();
        EnemyAttack.Attack();
        yield return new WaitForSeconds(attackRecoverTime);
        if (Random.value < .25f)
            goto backoff;
        goto lookForPlayer;

    backoff:
        Move(-movementSpeed);
        yield return new WaitForSeconds(.25f + Random.value * .25f);
        Move(0f);
        goto lookForPlayer;

    approach:
        LookAtTarget();
        Move(movementSpeed);
        for (float f = Random.value * .4f; f < .75f; f += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            if (Mobile.OnEdge)
            {
                Move(0f);
                if (Random.value < .5f)
                    goto attack;
                else
                    Move(-movementSpeed);
            }
        }
        Move(0f);
        if (Random.value < .5f)
            goto attack;
        if (Random.value < .2f)
            goto backoff;
        goto wait;

    wait:
        yield return new WaitForSeconds(Random.value * .2f);
        goto lookForPlayer;



        void LookAtTarget()
        {
            if (target != null)
            {
                transform.SetForward(target.transform.position.x - transform.position.x);
            }
        }
        void Move(float speed)
        {
            Mobile.HMomentum = transform.Forward() * speed;
        }
    }
}
