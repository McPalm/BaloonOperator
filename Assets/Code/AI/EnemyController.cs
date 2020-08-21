using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Mobile Mobile { get; set; }
    Health Health { get; set; }

    float stunTime;
    bool IsStunned => Time.timeSinceLevelLoad < stunTime;

    // Start is called before the first frame update
    void Start()
    {
        Mobile = GetComponent<Mobile>();
        Health = GetComponent<Health>();
        if (Health)
        {
            Health.OnHurt += Health_OnHurt;
            Health.OnChangeTrueHealth += Health_OnChangeTrueHealth;
        }
    }

    private void Health_OnChangeTrueHealth(int hp, int change)
    {
        if(change > 0)
            stunTime = Time.timeSinceLevelLoad + .5f;
    }

    private void Health_OnHurt(DamageData obj)
    {
        stunTime = Time.timeSinceLevelLoad + .5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(IsStunned)
        {
            Mobile.HMomentum = 0f;
            return;
        }
        if (Mobile.TouchingWallDirection == Mobile.Forward)
        {
            Mobile.FaceRight = !Mobile.FaceRight;
        }
        else if(Mobile.OnEdge)
        {
            Mobile.FaceRight = !Mobile.FaceRight;
        }
        Mobile.HMomentum = Mobile.Forward * 3;
    }
}
