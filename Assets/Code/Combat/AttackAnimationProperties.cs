using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationProperties : MonoBehaviour
{
    public PlatformingCharacter Player;
    public Animator Animator;

    // Update is called once per frame
    void FixedUpdate()
    {
        Animator.SetBool("Grounded", Player.Grounded);
    }
}
