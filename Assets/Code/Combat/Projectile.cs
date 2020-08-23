using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float gravity = 18f;
    public Vector2 speed;
    public Vector2 randomness;

    public float lifetime = 60f;

    private void Awake()
    {
        speed = new Vector2(speed.x + randomness.x * Random.value, speed.y + randomness.y * Random.value);
    }

    private void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;
        if (lifetime < 0f)
            Destroy(gameObject);
        speed = new Vector2(speed.x, speed.y - gravity * Time.fixedDeltaTime);
        transform.position += new Vector3(speed.x * transform.Forward(), speed.y) * Time.fixedDeltaTime;
    }
}
