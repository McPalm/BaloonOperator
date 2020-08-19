using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IInputReader
{
    public InputToken InputToken { get; set; }
    public Animator animator;
    PlatformingCharacter PC;
    public int attackDuration = 15;
    float cooldown = 0f;

    public bool CanAttack => PC.WallSliding == false && PC.Climbing == false && cooldown < Time.timeSinceLevelLoad;

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
            PC.Root(attackDuration);
            cooldown = Time.timeSinceLevelLoad + attackDuration * Time.fixedDeltaTime;
            OnAttack?.Invoke();
        }
    }
}
