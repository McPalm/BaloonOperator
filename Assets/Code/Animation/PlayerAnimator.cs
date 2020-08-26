using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlatformingCharacter PC;
    public AnimationPhysics AnimationPhysics;
    public SpriteRenderer SpriteRenderer;
    public SpriteSheet SpriteSheet;

    float runtime = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (AnimationPhysics.reachX != 0 || AnimationPhysics.reachY != 0)
        {
            Set(AnimationPhysics.reachX, AnimationPhysics.reachY);
        }
        else
        {

            if (PC.Grounded)
            {
                var speed = Mathf.Abs(PC.HMomentum);
                if (speed > 0f)
                {
                    runtime += Time.deltaTime * speed * 2f;
                    int index = (int)runtime % 8;
                    Set(index + 3);
                }
                else
                {
                    runtime = 0f;
                    Set(0);
                }
            }
            else
            {
                if (PC.VMomentum > 0f)
                    Set(1);
                else
                    Set(2);
            }
        }

    }

    void Set(int index) => SpriteRenderer.sprite = SpriteSheet.sprites[index];

    void Set(int x, int y)
    {
        if (y < 0)
            Set(12);
        if (y > 0)
            Set(17);
        if (x > 0)
            Set(15);
    }

}
