using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IInputReader
{
    public InputToken InputToken { get; set; }
    public Animator animator;
    PlatformingCharacter PC;
    float clearFlag = 0f;

    readonly float inputBuffer = .25f;

    public bool CanAttack => PC.WallSliding == false && PC.Climbing == false;

    public event System.Action OnAttack;

    void Start()
    {
        PC = GetComponent<PlatformingCharacter>();
    }

    void FixedUpdate()
    {
        if(CanAttack && InputToken.UsePressed)
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
    }
}
