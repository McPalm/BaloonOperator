using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Searches out enemies from the whole map and flies right towards them.
// Doesn't pass walls.
public class TellyAI : EnemyController
{
    PlatformingCharacter currentTarget;

    public override void InitAI()
    {
        Mobile.Gravity = 0;
        StartCoroutine(SearchForTarget());
    }

    IEnumerator SearchForTarget()
    {
        while (true)
        {
            currentTarget = FindTarget(0);
            if (currentTarget != null)
            {
                Debug.Log("FOUND TARGET!" + currentTarget.gameObject.name);
            }
            else
            {
                Debug.Log("DIDN't FIND TARGET");
            }
            yield return new WaitForSeconds(5f);
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
            Vector3 direction = Vector3.Normalize(transform.position - currentTarget.transform.position);

            Mobile.HMomentum = direction.x * -1;
            Mobile.VMomentum = direction.y * -1;
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
            Mobile.HMomentum = Mobile.Forward * 3;
        }
    }
}
