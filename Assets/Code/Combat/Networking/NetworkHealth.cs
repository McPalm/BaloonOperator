using Mirror;
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
        Debug.Log($"Health.blockTriggers: {Health.blockTriggers}");

        FindObjectOfType<MyNetworkManager>().E_OnServerReady += NetworkHealth_E_OnServerReady;
    }

    private void OnDestroy()
    {
        FindObjectOfType<MyNetworkManager>().E_OnServerReady -= NetworkHealth_E_OnServerReady;
    }

    private void Health_OnHurt(DamageData data) => Apply(data, false);
    private void Health_OnHeal(DamageData data) => Apply(data, true);

    private void Apply(DamageData data, bool heal)
    {
        int change = heal ? -data.damage : data.damage;
        var source = data.source.GetComponent<NetworkIdentity>();

        bool sourceIsPlayer = false;
        bool sourceIsEnvorment = false;
        bool sourceIsEnemy = false;


        if (source == null)
            sourceIsEnvorment = true;
        else
        {
            if (source.GetComponent<NetworkControls>() == null)
                sourceIsEnemy = true;
            else
                sourceIsPlayer = true;
        }

        Debug.Log($"Heal or hurt! source is...  player{sourceIsPlayer} enviroment{sourceIsEnvorment} enemy{sourceIsEnemy}", gameObject);

        if (isLocalPlayer)
        {
            if (sourceIsEnvorment)
                CmdChangeHealth(change);
            else if (sourceIsPlayer && source.isLocalPlayer)
                CmdChangeHealth(change);
            else if (sourceIsEnemy)
                CmdChangeHealth(change);
        }
        else
        {
            if(serverObject && isServer)
            {
                if (sourceIsEnemy || sourceIsEnvorment)
                    CmdChangeHealth(change);
            }
            if (sourceIsPlayer && source.isLocalPlayer)
                CmdChangeHealth(change);
        }
    }

    [Command(channel = Channels.DefaultReliable, ignoreAuthority = true)]
    void CmdChangeHealth(int change)
    {
        Debug.Log($"Healh change incomming {change}", gameObject);
        ServerChangeHealth(change);
    }

    void ServerChangeHealth(int change)
    {
        trueDamage += change;
        trueDamage = Mathf.Max(trueDamage, 0);
        RpcSetHealth(change, trueDamage, NetworkTime.time);
    }


    // sends update from the server to all clients
    [ClientRpc(channel = Channels.DefaultReliable)]
    void RpcSetHealth(int change, int lostHealth, double time)
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

    // sync health when a new player is connecting // TODO, doesn't actually work
    private void NetworkHealth_E_OnServerReady(NetworkConnection obj) => TargetStartupSync(obj, Health.HealthLost);
    [TargetRpc]void TargetStartupSync(NetworkConnection target, int lostHealth) => Health.SetHealth(lostHealth, 0);
}
