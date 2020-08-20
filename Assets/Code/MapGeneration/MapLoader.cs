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
        MapModule[] generatedMap = null;
        for(int i = 0; i < 100; i++)
        {
            try
            {
                generatedMap = generator.GenerateMap();
                break;
            }
            catch
            {
                Debug.Log($"Failed Attempt {i}");
            }

        }

        MapPainter.Paint(generatedMap);

    }

}
