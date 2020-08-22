using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkEnemy : NetworkBehaviour
{
    Mobile Mobile;
    double lastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        Mobile = GetComponent<Mobile>();
    }

    private void FixedUpdate()
    {
        if (isServer)
        {
            RpcSyncInput(transform.position, new Vector2(Mobile.HMomentum, Mobile.VMomentum), NetworkTime.time);
        }

    }

    [ClientRpc(channel = Channels.DefaultUnreliable)] private void RpcSyncInput(Vector2 pos, Vector2 force, double time)
    {
        if (isServer)
            return;
        if (lastUpdate > time)
            return;
        lastUpdate = time;
        transform.position = pos;
        Mobile.HMomentum = force.x;
        Mobile.VMomentum = force.y;
    }
}
