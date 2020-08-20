using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapModulePainter : MonoBehaviour
{
    public bool save;
    public bool load;
    public MapModuleSample currentSample;
    public Tilemap tilemap;


    private void OnDrawGizmosSelected()
    {
        if(save)
        {
            currentSample.CopyFrom(tilemap, 0, 0);
            save = false;
        }
        else if(load)
        {
            currentSample.PaintTo(tilemap, 0, 0);
            load = false;
        }
    }
}
