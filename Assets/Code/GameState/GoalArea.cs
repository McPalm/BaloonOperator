﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public float radius;
    GameManager GameManager { set; get; }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        while (GameManager == null)
        {
            yield return null;
            GameManager = FindObjectOfType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int count=0;
        int alive = 0;
        if (GameManager.AllPlayers.Count == 0)
        {
            return;
        }

        foreach (GameObject go in GameManager.AllPlayers)
        {
            if (Vector3.Distance(go.transform.position, transform.position) < radius)
            {
                count++;
            }
            if (go.GetComponent<Health>().CurrentHealth > 0)
            {
                alive++;
            }
        }
        if (count >= alive)
        {
            GameManager.TriggerWin();
        }
    }
}
