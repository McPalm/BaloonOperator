using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRelay : MonoBehaviour
{
    public AudioClip clip;
    public float volume = 0f;

    public void PlaySound()
    {
        AudioPool.PlaySound(transform.position, clip, volume);
    }
}
