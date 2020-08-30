using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ObjectSpawner
{
    // Start is called before the first frame update
    new void Start()
    {
        GameObject = new GameObject[]
        {
            FindObjectOfType<EnemyPalette>().GetRandom(),
        };
        // config the spawn list
        base.Start();
    }
    
}
