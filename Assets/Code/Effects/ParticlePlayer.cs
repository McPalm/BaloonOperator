using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlayer : MonoBehaviour
{
    public ParticleRef refference;

    public void Play(Vector3 position)
    {
        transform.position = position;
        GetComponent<ParticleSystem>().Play();
    }
}
