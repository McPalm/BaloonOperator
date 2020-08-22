using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Runtime.CompilerServices;
using System.Linq;

public class MapLoader : NetworkBehaviour
{
    struct SerializedMapModule
    {
        public int index;
        public bool flip;
    }

    bool generated = false;

    public MapModuleSample[] mapModuleSamples;
    public MapModuleSet[] mapModuleSets;
    public MapPainter MapPainter;

    SerializedMapModule[] serializedArray;

    List<MapModuleSample> modules = new List<MapModuleSample>();
    MapGenerator generator;

    private void Awake()
    {
        modules.AddRange(mapModuleSamples);
        foreach (var set in mapModuleSets)
            modules.AddRange(set.MapModules);
    }

    MapModuleSample[] GetSamplesForDifficulty(int difficulty)
    {
        Debug.Log($"Getting samples for diffuclty {difficulty}");
        difficulty = Mathf.Clamp(difficulty, 0, mapModuleSets.Length-1);
        var modules = new List<MapModuleSample>(mapModuleSets[difficulty].MapModules);
        if (difficulty - 1 >= 0)
            modules.AddRange(GetSome(.33f, mapModuleSets[difficulty - 1].MapModules));
        if (difficulty + 1 < mapModuleSets.Length)
            modules.AddRange(GetSome(.33f, mapModuleSets[difficulty + 1].MapModules));
        Debug.Log($"Final module count at {modules.Count}");
        return modules.ToArray();
    }

    MapModuleSample[] GetSome(float chance, List<MapModuleSample> from)
    {
        return from.Where(a => Random.value < chance).ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            generator = new MapGenerator()
            {
                modules = GetSamplesForDifficulty(GameManager.StageDifficulty),
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
            serializedArray = new SerializedMapModule[16];
            for(int i = 0; i < 16; i++)
            {
                serializedArray[i].index = modules.IndexOf(generatedMap[i].MapModuleSample);
                serializedArray[i].flip = generatedMap[i].flip;
            }
            MapPainter.Paint(generatedMap);
            RpcPaintMap(serializedArray);
            generated = true;
        }
        else
        {
            CmdRequestSync();
        }
    }

    [Command(channel = Channels.DefaultReliable, ignoreAuthority = true)]
    void CmdRequestSync()
    {
        RpcPaintMap(serializedArray);
    }

    [ClientRpc(channel = Channels.DefaultReliable)]
    private void RpcPaintMap(SerializedMapModule[] serializedMap)
    {
        if (generated)
            return;
        MapModule[] generatedMap = new MapModule[16];
        for (int i = 0; i < 16; i++)
        {
            generatedMap[i] = new MapModule(modules[serializedMap[i].index], serializedMap[i].flip);
        }
        MapPainter.Paint(generatedMap);
        generated = true;
    }
}