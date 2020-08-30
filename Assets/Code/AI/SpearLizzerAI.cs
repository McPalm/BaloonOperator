using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearLizzerAI : EnemyController
{
    public float attackRange = 8f;
    public float attackRecovery = 1f;

    public EnemyAttack Attack;

    public override void InitAI()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator AwakeCoroutine()
    {
        yield return new WaitForSeconds(1f);

        PlatformingCharacter target = null;

        for (; ; )
        {
            target = FindTarget(attackRange);
            if (target != null)
            {
                LookAtTarget();
                yield return new WaitForSeconds(Random.value * .5f);
                LookAtTarget();
                Attack.Attack();
                yield return new WaitForSeconds(attackRecovery);
            }
            else
                yield return new WaitForSeconds(Random.value * .5f);
        }

        void LookAtTarget()
        {
            if (target != null)
            {
                transform.SetForward(target.transform.position.x - transform.position.x);
            }
        }
    }
}
