using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ParticleRef : ScriptableObject
{
    public void Play(Vector3 position)
    {
        FindObjectOfType<ParticleManager>().Play(this, position);
    }
}
