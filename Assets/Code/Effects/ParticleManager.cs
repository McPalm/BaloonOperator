using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    ParticlePlayer[] players;

    void Start()
    {
        players = GetComponentsInChildren<ParticlePlayer>();
    }

    public void Play(ParticleRef particle, Vector3 position)
    {
        foreach (var player in players)
        {
            if (player.refference == particle)
            {
                player.Play(position);
                return;
            }
        }
    }
}
