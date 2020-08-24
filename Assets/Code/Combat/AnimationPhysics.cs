using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPhysics : MonoBehaviour
{
    public PlatformingCharacter player;
    public Mobile mobile;

    public float xForce;
    public float yForce;

    public bool rooted;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player)
        {
            if (rooted)
                player.Root(2);
        }
        if(mobile)
        {
            if (xForce != 0f)
                mobile.HMomentum = xForce * mobile.Forward;
            if (yForce != 0f)
                mobile.VMomentum = yForce;
        }
    }
}
