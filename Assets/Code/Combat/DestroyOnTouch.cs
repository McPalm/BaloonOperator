using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    public LayerMask LayerMask;

    public float radius = .2f;

    RaycastHit2D hit;

    public event System.Action OnTouch;

    // Update is called once per frame
    void FixedUpdate()
    {
        hit = Physics2D.CircleCast(transform.position, radius, direction: Vector2.zero, layerMask: LayerMask, distance: 10f);
        if (hit)
        {
            OnTouch?.Invoke();
            Destroy(gameObject);
        }
    }
}
