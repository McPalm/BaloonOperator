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
        var obj = Instantiate(GameObject, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.parent = null;
        GetComponent<SpriteRenderer>().enabled = false;
        NetworkServer.Spawn(obj);
    }

}
