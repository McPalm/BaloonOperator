using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager game = FindObjectOfType<GameManager>();
        game.Player.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
