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
    
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader = GetComponent<SceneLoader>();
        SceneLoader.LoadScene(startLevel);

        ///Move character to the right position
    }

    public void ResetScene()
    {
        SceneLoader.ReloadScene();
        ///Probably add reset health to character since they aren't a part of the scene.
        ///Move player to the right starting position.
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }

}
