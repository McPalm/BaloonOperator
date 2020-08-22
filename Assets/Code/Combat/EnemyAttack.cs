using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : NetworkBehaviour
{
    /// <summary>
    ///  perform an attack, network synced
    /// </summary>
    public void Attack() => RpcAttack();
    /// <summary>
    ///  perform an attack with a delay, will ensure it happens simultaneously on different clients, and with some lag protection, if you want consequetive syncronized attacks I suppose.
    /// </summary>
    /// <param name="delay"></param>
    public void AttackIn(float delay) => RpcAttackIn(NetworkTime.time + delay);

    public Animator Animator;

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcAttack()
    {
        Animator.SetTrigger("Strike");
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcAttackIn(double time)
    {
        float delay = (float)(time - NetworkTime.time);
        StartCoroutine(AttackInCoroutine(delay));
    }

    IEnumerator AttackInCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Animator.SetTrigger("Strike");
    }
}
