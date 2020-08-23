using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkAutoSpawn : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkServer.Spawn(gameObject);
    }
}
