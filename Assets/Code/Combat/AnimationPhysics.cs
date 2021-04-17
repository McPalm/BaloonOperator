using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPhysics : MonoBehaviour
{
    public bool easyCancel;

    public PlatformingCharacter player;
    public Mobile mobile;

    public float xForce;
    public float yForce;
    public bool airForce;

    public bool rooted;
    [Range(0f, 1f)]
    public float friction = 1f;

    int impulseDuration = 0;
    bool impulseApplies;

    public int reachX;
    public int reachY;

    public event System.Action OnStartSwing;

    public void StartSwing()
    {
        OnStartSwing?.Invoke();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player)
        {
            if (rooted)
                player.Root(2);
            player.Friction = friction;
        }
        if (mobile)
        {
            if (xForce == 0f && yForce == 0f)
            {
                impulseDuration = 0;
                impulseApplies = false;
            }
            else impulseDuration++;
            if (impulseDuration == 1 && mobile.Grounded)
                impulseApplies = true;

            if (impulseApplies || airForce)
            {
                if (xForce != 0f)
                    mobile.HMomentum = xForce * mobile.Forward;
                if (yForce != 0f)
                    mobile.VMomentum = yForce;
            }

        }
    }
}
