using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] GameObject;
    // Start is called before the first frame update
    public void Start()
    {
        var obj = GameObject[Random.Range(0, GameObject.Length)];

        if (obj != null && MyNetworkManager.isServer)
        {

            obj = Instantiate(obj, transform.position, Quaternion.identity);
            obj.transform.parent = transform;
            obj.transform.parent = null;
            NetworkServer.Spawn(obj);
        }
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
