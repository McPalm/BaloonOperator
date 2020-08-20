using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{

    public MapModuleSample[] mapModuleSamples;
         

    public MapPainter MapPainter;

    MapGenerator generator;


    // Start is called before the first frame update
    void Start()
    {
        generator = new MapGenerator()
        {
            modules = mapModuleSamples
        };
        var generatedMap = generator.GenerateMap();

        MapPainter.Paint(generatedMap);

    }

}
