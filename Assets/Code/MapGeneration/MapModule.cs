using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapModule
{
    public MapModuleSample MapModuleSample { get; set; }
    public bool flip;

    public MapModule(MapModuleSample sample, bool flip)
    {
        MapModuleSample = sample;
        this.flip = flip;
    }

    public void PaintTo(Tilemap tilemap, int x, int y)
    {
        MapModuleSample.PaintTo(tilemap, x, y, flip);
    }

}
