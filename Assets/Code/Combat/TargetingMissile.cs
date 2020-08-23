using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingMissile : MonoBehaviour
{
    public float speed;
    public Transform target;

    Vector3 delta;

    public float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if(target)
        {
            transform.right = target.position - transform.position;
            delta = (target.position - transform.position).normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            Debug.LogWarning("Missing projectile!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0f)
            Destroy(gameObject);
        transform.position += delta;
    }
}
