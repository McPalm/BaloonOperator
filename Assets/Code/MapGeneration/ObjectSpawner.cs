using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject GameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (MyNetworkManager.isServer)
        {

            var obj = Instantiate(GameObject, transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            obj.transform.parent = null;
            NetworkServer.Spawn(obj);
        }
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
