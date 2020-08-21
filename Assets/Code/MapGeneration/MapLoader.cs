using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Runtime.CompilerServices;

public class MapLoader : NetworkBehaviour
{
    struct SerializedMapModule
    {
        public int index;
        public bool flip;
    }


    public MapModuleSample[] mapModuleSamples;
    public MapModuleSet[] mapModuleSets;

    public MapPainter MapPainter;

    List<MapModuleSample> modules = new List<MapModuleSample>();

    MapGenerator generator;

    private void Awake()
    {
        modules.AddRange(mapModuleSamples);
        foreach (var set in mapModuleSets)
            modules.AddRange(set.MapModules);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            generator = new MapGenerator()
            {
                modules = modules.ToArray()
            };
            MapModule[] generatedMap = null;
            for (int i = 0; i < 100; i++)
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
            SerializedMapModule[] serializedArray = new SerializedMapModule[16];
            for(int i = 0; i < 16; i++)
            {
                serializedArray[i].index = modules.IndexOf(generatedMap[i].MapModuleSample);
                serializedArray[i].flip = generatedMap[i].flip;
            }
            MapPainter.Paint(generatedMap);
            RpcPaintMap(serializedArray);
        }

    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    private void RpcPaintMap(SerializedMapModule[] serializedMap)
    {
        MapModule[] generatedMap = new MapModule[16];
        for (int i = 0; i < 16; i++)
        {
            generatedMap[i] = new MapModule(modules[serializedMap[i].index], serializedMap[i].flip);
        }
        MapPainter.Paint(generatedMap);
    }
}