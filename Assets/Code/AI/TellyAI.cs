using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Searches out enemies from the whole map and flies right towards them.
// Doesn't pass walls.
public class TellyAI : EnemyController
{
    public float speed = 2f;
    public float targetRange = 2f;

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
            currentTarget = FindTarget(targetRange);
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
            Vector3 direction = Vector3.Normalize(currentTarget.transform.position - transform.position);

            Mobile.HMomentum = direction.x * speed;
            Mobile.VMomentum = direction.y * speed;
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
            Mobile.HMomentum = Mobile.Forward * speed;
        }
    }
}
