using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPhysics : MonoBehaviour
{
    public PlatformingCharacter player;
    public Mobile mobile;

    public float xForce;
    public float yForce;
    public bool airForce;

    public bool rooted;

    int grace = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player)
        {
            if (rooted)
                player.Root(2);
        }
        if (mobile)
        {

            grace = mobile.Grounded ? 5 : grace - 1;
            if (grace > 0 || airForce)
            {
                if (xForce != 0f)
                    mobile.HMomentum = xForce * mobile.Forward;
                if (yForce != 0f)
                    mobile.VMomentum = yForce;
            }
        }
    }
}
