using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flinch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        health.OnHurt += Health_OnHurt;
    }

    private void Health_OnHurt(DamageData obj)
    {
        if (obj.reject)
            return;
        var player = GetComponent<PlatformingCharacter>();
        player.VMomentum = 8f;
        float direction = 0f;
        if(obj.source != null)
        {
            direction = transform.position.x - obj.source.transform.position.x;
            direction = Mathf.Sign(direction) * 12f;
            player.HMomentum = direction;
        }
        player.DisabledFrames = 30;
        StartCoroutine(FlinchAnimation(GetComponent<Attack>().animator));
    }

    IEnumerator FlinchAnimation(Animator animator)
    {
        animator.SetBool("Flinching", true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Flinching", false);
    }
}
