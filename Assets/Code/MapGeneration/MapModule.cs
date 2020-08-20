using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModule
{
    public MapModuleSample MapModuleSample { get; set; }
    bool flip;

    public MapModule(MapModuleSample sample, bool flip)
    {
        MapModuleSample = sample;
        this.flip = flip;
    }
}
