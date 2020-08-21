using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{

    public MapModuleSample[] mapModuleSamples;
    public MapModuleSet[] mapModuleSets;

    public MapPainter MapPainter;

    MapGenerator generator;


    // Start is called before the first frame update
    void Start()
    {
        List<MapModuleSample> modules = new List<MapModuleSample>();
        modules.AddRange(mapModuleSamples);
        foreach (var set in mapModuleSets)
            modules.AddRange(set.MapModules);
        generator = new MapGenerator()
        {
            modules = modules.ToArray()
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
