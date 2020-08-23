using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float gravity = 18f;
    public Vector2 speed;

    public float lifetime = 60f;

    private void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0f)
            Destroy(gameObject);
        speed = new Vector2(speed.x, speed.y - gravity * Time.fixedDeltaTime);
        transform.position += new Vector3(speed.x * transform.Forward(), speed.y) * Time.fixedDeltaTime;
    }
}
