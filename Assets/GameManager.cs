using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [Scene]
    public string startLevel;

    public SceneLoader SceneLoader { get; set; }
    public GameObject Player { get; set; }
    public List<GameObject> AllPlayers { get; set; }

    private void Awake()
    {
        AllPlayers = new List<GameObject>();
    }

    // Start is called before the first frame update
    [Server]
    void Start()
    {
        SceneLoader = GetComponent<SceneLoader>();
        SceneLoader.LoadScene(startLevel);
    }

    [Server]
    public void Update()
    {
        bool isGameOver = true;
        if (AllPlayers.Count > 0 )
        {
            foreach (GameObject go in AllPlayers)
            {
                if (go.GetComponent<Health>().CurrentHealth > 0)
                {
                    isGameOver = false;
                }
            }
            if (isGameOver)
            {
                ResetScene();
            }
        }
    }

    [Server]
    public void ResetScene()
    {
        SceneLoader.ReloadScene();
        foreach (GameObject go in AllPlayers)
        {
            go.GetComponent<Health>().Heal(999, true);
        }
        ///Probably add reset health to character since they aren't a part of the scene.
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }

    public void RegisterAllPlayers(GameObject player)
    {
        AllPlayers.Add(player);
    }

}
