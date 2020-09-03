using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    [Scene]
    public string tutorialLoad;
    [Scene]
    public string gameLoad;
    public TMPro.TMP_InputField networkAdress;

    private void Start()
    {
        var networkManager = FindObjectOfType<NetworkManager>();
        networkAdress.text = networkManager.networkAddress;
    }


    public void PlayTutorial()
    {
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.onlineScene = tutorialLoad;
        networkManager.StartHost();
    }

    public void HostGame()
    {
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.onlineScene = gameLoad;
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        var networkManager = FindObjectOfType<NetworkManager>();
        networkManager.networkAddress = networkAdress.text;
        networkManager.StartClient();
    }
}
