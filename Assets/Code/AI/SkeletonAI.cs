using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

public class SkeletonAI : EnemyController
{
    PlatformingCharacter currentTarget;
    
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
        while (true)
        {
            yield return new WaitForSeconds(5f);
            currentTarget = FindTarget(10);
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
        while (true)
        {
            movement = NextMovement;
            yield return new WaitForSeconds(0.25f);
            movement = 0;
            yield return new WaitForSeconds(1f);
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
            Debug.Log("Movement:" + Mobile.HMomentum);
        }
    }
}
