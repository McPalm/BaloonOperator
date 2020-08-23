using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class SkeletonAI : EnemyController
{
    PlatformingCharacter currentTarget;
    public EnemyAttack Attack;
    public float sightRange = 5f;
    
    int[] movementPattern = { 4, -2, -4, 2 };
    int index;
    int NextMovement
    {
        get
        {
            if (index == movementPattern.Length)
            {
                index = 0;
            }
            return movementPattern[index++];
        }
    }
    int movement;

    public override void InitAI()
    {
        StartCoroutine(SearchForTarget());
        StartCoroutine(GetMovement());
        index = 0;
    }

    IEnumerator SearchForTarget()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.value * .25f);
            currentTarget = FindTarget(sightRange);
            if (currentTarget != null)
            {
                Debug.Log("FOUND TARGET!" + currentTarget.gameObject.name);
            }
            else
            {
                Debug.Log("DIDN't FIND TARGET");
            }
        }
    }

    IEnumerator GetMovement()
    {
        while (enabled)
        {
            if (currentTarget != null)
            { 
                transform.SetForward(currentTarget.transform.position.x - transform.position.x);
                movement = NextMovement;
                yield return new WaitForSeconds(0.25f);
                movement = 0;
                while(Random.value < .7f && currentTarget != null && enabled)
                {
                    Debug.Log("Toss me a thing!");
                    Attack.Attack();
                    yield return new WaitForSeconds(.5f);
                }
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                movement = 0;
                yield return null;
            }
        }
    }

    public override void Enemybehaviour()
    {
        if (IsStunned)
        {
            Mobile.HMomentum = 0f;
            return;
        }
        else
        {
            if (Mobile.TouchingWallDirection == Mobile.Forward)
            {
                Mobile.FaceRight = !Mobile.FaceRight;
            }
            else if (Mobile.OnEdge)
            {
                Mobile.FaceRight = !Mobile.FaceRight;
            }
            Mobile.HMomentum = Mobile.Forward * 1 * movement;
        }
    }
}
