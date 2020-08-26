using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShellAI : EnemyController
{
    public override void InitAI()
    {
    }

    protected override IEnumerator AwakeCoroutine()
    {
        for (; ; )
        {

            yield return new WaitForFixedUpdate();
            {
                if (IsStunned)
                {
                    Mobile.HMomentum = 0f;
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
    }
}
