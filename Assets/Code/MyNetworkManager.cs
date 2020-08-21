using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public event System.Action<NetworkConnection> E_OnServerReady;

    static public bool isServer = false;

    public override void OnStartServer()
    {
        isServer = true;
        base.OnStartServer();
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
}