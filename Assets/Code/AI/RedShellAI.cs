using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedShellAI : EnemyController
{
    public override void InitAI()
    {
    }

    public override void Enemybehaviour()
    {
        if (IsStunned)
        {
            Mobile.HMomentum = 0f;
            return;
        }
        if (Mobile.TouchingWallDirection == Mobile.Forward)
        {
            Mobile.FaceRight = !Mobile.FaceRight;
        }
        else if (Mobile.OnEdge)
        {
            Mobile.FaceRight = !Mobile.FaceRight;
        }
        Debug.Log($"speed {Mobile.HMomentum}");
        Mobile.HMomentum = Mobile.Forward * 3;
        Debug.Log($"speed {Mobile.HMomentum}");
    }

}
