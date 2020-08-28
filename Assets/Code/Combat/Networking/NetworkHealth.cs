using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class NetworkHealth : NetworkBehaviour
{
    Health Health { get; set; }
    double lastUpdate; // discard health updates that happened before this.
    int trueDamage; // maintained by the server, ths the anbsolute source of healthLoss;

    bool serverObject;
    bool isPlayer;

    private void Start()
    {
        Health = GetComponent<Health>();
        Health.OnHurt += Health_OnHurt;
        Health.OnHeal += Health_OnHeal;

        isPlayer = GetComponent<NetworkControls>();

        if (isPlayer)
        {
            Health.blockTriggers = !hasAuthority;
            serverObject = false;
        }
        else
        {
            Health.blockTriggers = !isServer;
            serverObject = true;
        }

        if(!isServer)
            CmdSyncHealth();
    }

    private void Health_OnHurt(DamageData data) => Apply(data, false);
    private void Health_OnHeal(DamageData data) => Apply(data, true);

    private void Apply(DamageData data, bool heal)
    {
        int change = heal ? -data.damage : data.damage;
        if(data.authorative)
        {
            if (!isServer)
                throw new System.Exception("Only server is allowed to do authorative heal and damage");
            CmdChangeHealth(change, data.GetProps());
            return;
        }
        var source = data.source.GetComponent<NetworkIdentity>();

        bool sourceIsPlayer = false;
        bool sourceIsEnvorment = false;
        bool sourceIsEnemy = false;
        bool accept = false;

        if (source == null)
            sourceIsEnvorment = true;
        else
        {
            if (source.GetComponent<NetworkControls>() == null)
                sourceIsEnemy = true;
            else
                sourceIsPlayer = true;
        }

        if (isLocalPlayer)
        {
            if (sourceIsEnvorment || sourceIsEnemy)
                accept = true;
            if (sourceIsPlayer && source.isLocalPlayer)
                accept = true;
        }
        else
        {
            if (serverObject && isServer)
            {
                if (sourceIsEnemy || sourceIsEnvorment)
                    accept = true;
            }
            if (sourceIsPlayer && source.isLocalPlayer)
                accept = true;
        }
        if (accept)
            CmdChangeHealth(change, data.GetProps());
        data.reject = !accept;
    }

    [Command(channel = Channels.DefaultReliable, ignoreAuthority = true)]
    void CmdChangeHealth(int change, DamageProperties props)
    {
        ServerChangeHealth(change, props);
    }

    void ServerChangeHealth(int change, DamageProperties props)
    {
        trueDamage += change;
        trueDamage = Mathf.Max(trueDamage, 0);
        RpcSetHealth(trueDamage, props, NetworkTime.time);
    }


    // sends update from the server to all clients
    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcSetHealth(int lostHealth, DamageProperties props, double time)
    {
        if(time < lastUpdate)
        {
            Health.SetHealth(Health.HealthLost, props);
        }
        else
        {
            Health.SetHealth(lostHealth, props);
            lastUpdate = time;
        }
    }


    [Command(channel = Channels.DefaultReliable, ignoreAuthority = true)]private void CmdSyncHealth() => RpcSetHealth(trueDamage, new DamageProperties(), NetworkTime.time);
}
