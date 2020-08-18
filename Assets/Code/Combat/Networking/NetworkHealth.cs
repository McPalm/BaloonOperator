using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class NetworkHealth : NetworkBehaviour
{
    Health Health { get; set; }
    double lastUpdate;

    private void Start()
    {
        Health = GetComponent<Health>();
        Health.OnHurt += Health_OnHurt;
        Health.OnHeal += Health_OnHeal;
        Health.blockTriggers = !hasAuthority;
        Debug.Log($"hasAuthority: {hasAuthority}");
    }

    private void Health_OnHeal(int damage)
    {
        if(isServer)
        {
            RpcHealthUpdate(damage, Health.HealthLost, NetworkTime.time);
        }
        else
        {
            CmdHealthUpdate(damage);
        }
    }

    private void Health_OnHurt(int damage)
    {
        if(isServer)
        {
            RpcHealthUpdate(-damage, Health.HealthLost, NetworkTime.time);
        }
        else
        {
            CmdHealthUpdate(-damage);
        }
    }

    [Command(channel = Channels.DefaultReliable, ignoreAuthority = true)]
    void CmdHealthUpdate(int change)
    { 
        if(change < 0)
        {
            Health.Hurt(-change);
        }
        else
        {
            Health.Heal(change);
        }
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcHealthUpdate(int change, int lostHealth, double time)
    {
        if(time < lastUpdate)
        {
            Health.SetHealth(Health.HealthLost, change);
        }
        else
        {
            Health.SetHealth(lostHealth, change);
            lastUpdate = time;
        }
    }
}
