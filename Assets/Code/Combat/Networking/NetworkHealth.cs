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

    private void Start()
    {
        Health = GetComponent<Health>();
        Health.OnHurt += Health_OnHurt;
        Health.OnHeal += Health_OnHeal;

        bool isPlayer = GetComponent<NetworkControls>();

        if (isPlayer)
            Health.blockTriggers = !hasAuthority;
        else
            Health.blockTriggers = !isServer;
        Debug.Log($"Health.blockTriggers: {Health.blockTriggers}");

        FindObjectOfType<MyNetworkManager>().E_OnServerReady += NetworkHealth_E_OnServerReady;
    }

    private void OnDestroy()
    {
        FindObjectOfType<MyNetworkManager>().E_OnServerReady -= NetworkHealth_E_OnServerReady;
    }

    private void Health_OnHurt(int damage) => CmdChangeHealth(damage);
    private void Health_OnHeal(int damage) => CmdChangeHealth(-damage);

    [Command(channel = Channels.DefaultReliable)]
    void CmdChangeHealth(int change)
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
