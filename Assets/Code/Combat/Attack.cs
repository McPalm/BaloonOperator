using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IInputReader
{
    public InputToken InputToken { get; set; }
    public Animator animator;
    public AnimationPhysics animationPhysics;
    PlatformingCharacter PC;
    float clearFlag = 0f;
    Stamina Stamina { get; set; }

    readonly float inputBuffer = .25f;

    public bool CanAttack => PC.WallSliding == false && PC.Climbing == false;

    public event System.Action OnAttack;

    void Start()
    {
        Stamina = GetComponent<Stamina>();
        PC = GetComponent<PlatformingCharacter>();
        animationPhysics.OnStartSwing += () => Stamina.Spend(.2f);
    }

    void FixedUpdate()
    {
        if(CanAttack && InputToken.UsePressed && Stamina.HasStamina)
        {
            animator.SetTrigger("Strike");
            InputToken.ConsumeUse();
            clearFlag = Time.timeSinceLevelLoad + inputBuffer;
            OnAttack?.Invoke();
        }
        else if(clearFlag > 0f && clearFlag < Time.timeSinceLevelLoad)
        {
            animator.ResetTrigger("Strike");
        }
        animator.SetBool("AttackHeld", InputToken.UseHeld);
        animator.SetBool("InputForward",  Mathf.RoundToInt(InputToken.Direction.x) == transform.Forward());
    }
}
