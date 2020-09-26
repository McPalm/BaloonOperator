using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Open.Nat;
using System.Threading;

public class MyNetworkManager : NetworkManager
{
    public event System.Action<NetworkConnection> E_OnServerReady;

    static public bool isServer = false;

    public override void OnStartServer()
    {
        isServer = true;
        base.OnStartServer();
        var butts = PortForwardAsync(7777);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        isServer = false;
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("New player connected!");
        base.OnServerReady(conn);
        E_OnServerReady?.Invoke(conn);
    }

    public async System.Threading.Tasks.Task PortForwardAsync(int port)
    {
        Debug.Log("Attempting Port Forward...");
        var discoverer = new NatDiscoverer();

        var cts = new CancellationTokenSource(10000);
        try
        {

            var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

            await device.CreatePortMapAsync(new Mapping(Protocol.Udp, port, port, "HorseCave"));
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
        Debug.Log("Port Forward attempt complete.");
    }
}