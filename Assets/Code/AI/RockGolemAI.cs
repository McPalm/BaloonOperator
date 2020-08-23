using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RockGolemAI : EnemyController
{
    public EnemyAttack Attack;
    public float targetRange = 5f;
    public float speed = 7f;

    PlatformingCharacter currentTarget;

    public override void InitAI()
    {
        StartCoroutine(SearchForTarget());
    }

    IEnumerator SearchForTarget()
    {
        while (true)
        {
            currentTarget = FindTarget(targetRange);
            if (currentTarget != null && (currentTarget.transform.position.y > transform.position.y - 1f|| currentTarget.transform.position.y < transform.position.y + 1f))
            {
            }
            else
            {
                currentTarget = null;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator Charge()
    {
        Attack.Attack();
        yield return new WaitForSeconds(0.25f);
        Mobile.HMomentum = Mobile.Forward * speed;
        yield return new WaitForSeconds(0.5f);
        currentTarget = null;
        yield break;
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
            transform.SetForward(currentTarget.transform.position.x - transform.position.x);
            StartCoroutine(Charge());
        }
        else
        {
            Mobile.HMomentum = 0f;
        }
    }

}
