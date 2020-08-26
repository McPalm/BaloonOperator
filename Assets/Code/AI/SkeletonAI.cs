using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class SkeletonAI : EnemyController
{
    public EnemyAttack Attack;
    public float sightRange = 5f;
    
    int[] movementPattern = { 4, -2, -4, 2 };
    int index;
    int NextMovement => movementPattern[index++ % movementPattern.Length];

    public override void InitAI()
    {
    }

    protected override IEnumerator AwakeCoroutine()
    {
        index = 0;
        yield return new WaitForFixedUpdate();
        for (; ; )
        {

            if (IsStunned)
            {
                Mobile.HMomentum = 0f;
                yield return null;
            }
            else
            {
                PlatformingCharacter currentTarget;
                currentTarget = FindTarget(sightRange);
                yield return Shuffle();
                if (currentTarget != null)
                {
                    transform.SetForward(currentTarget.transform.position.x - transform.position.x);
                    yield return ThrowBones();
                    yield return new WaitForSeconds(.5f);
                }
                else
                {
                    Mobile.HMomentum = 0;
                    yield return new WaitForSeconds(Random.value);
                }
            }
        }
    }

    IEnumerator Shuffle()
    {
        Mobile.HMomentum = NextMovement;
        yield return new WaitForSeconds(0.25f);
        Mobile.HMomentum = 0;
    }

    IEnumerator ThrowBones()
    {
        int maxToss = 3;
        while (Random.value < .7f && maxToss-- > 0)
        {
            Attack.Attack();
            yield return new WaitForSeconds(.5f);
        }
    }
}
