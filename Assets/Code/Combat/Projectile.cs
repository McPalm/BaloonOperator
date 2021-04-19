using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float gravity = 18f;
    public Vector2 speed;
    public Vector2 randomness;

    public float lifetime = 60f;
    float currentlife = 0f;

    public float airFriction = 1f;

    public bool fullRotation = false;

    public bool useGravityOverLifetime = false;
    public AnimationCurve GravityOverLifetime;

    private void Awake()
    {
        speed = new Vector2(speed.x + randomness.x * Random.value, speed.y + randomness.y * Random.value);
    }

    private void FixedUpdate()
    {
        currentlife += Time.fixedDeltaTime;
        if (currentlife > lifetime)
            Destroy(gameObject);
        if (useGravityOverLifetime)
            gravity = GravityOverLifetime.Evaluate(currentlife);
        if (fullRotation)
        {
            speed = new Vector2(speed.x, speed.y - gravity * Time.fixedDeltaTime);
            var move = transform.right * speed.x;
            move += Vector3.up * speed.y;
            transform.position += move * Time.fixedDeltaTime;

        }
        else
        {
            speed = new Vector2(speed.x, speed.y - gravity * Time.fixedDeltaTime);
            transform.position += new Vector3(speed.x * transform.Forward(), speed.y) * Time.fixedDeltaTime;
        }
        speed *= airFriction;
    }
}
