using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : EnemyController
{
    public float windupTime = .5f;
    public float chargeTime = .2f;

    public EnemyAttack Attack;
    public float targetRange = 5f;
    public float speed = 7f;
    public float engageRange = 5f;

    PlatformingCharacter currentTarget;

    public override void InitAI()
    {
    }


    IEnumerator Charge()
    {
        Attack.Attack();
        yield return new WaitForSeconds(windupTime);
        Mobile.HMomentum = Mobile.Forward * speed;
        yield return new WaitForSeconds(chargeTime);
        currentTarget = null;
        // keep going while in the air without braeking
        while (Mobile.Grounded == false)
            yield return null;
        // slow down over 10 frames
        for(int i = 0; i < 10f; i++)
        {
            Mobile.HMomentum *= .9f;
            yield return new WaitForFixedUpdate();
        }
        Mobile.HMomentum = 0f;
    }

    protected override IEnumerator AwakeCoroutine()
    {
        while (true)
        {
            while (!enabled)
            {
                yield return null;
            }
            currentTarget = FindTarget(targetRange);
            if (currentTarget != null)
            {

                float dx = Mathf.Abs(currentTarget.transform.position.x - transform.position.x);
                float dy = Mathf.Abs(currentTarget.transform.position.y - transform.position.y);
                if (dy < 3f)
                    transform.SetForward(currentTarget.transform.position.x - transform.position.x);
                if (dy < 1f && dx < engageRange)
                {
                    yield return Charge();
                }
            }
            else
            {
                currentTarget = null;
            }
            yield return new WaitForSeconds(Random.value * 0.2f);
        }
    }
}
