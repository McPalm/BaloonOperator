using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPainter : MonoBehaviour
{
    public Tilemap Tilemap;

    public void Paint(MapModule[] modules)
    {
        int i = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                modules[i++].PaintTo(Tilemap, x * 13, y * 13);
            }
        }
    }    
}
