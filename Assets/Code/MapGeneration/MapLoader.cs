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

    public GeneratorSet[] sets;
    public MapPainter MapPainter;

    SerializedMapModule[] serializedArray;

    public MapModuleList MapModuleList;
    MapGenerator generator;

    MapGenerator GetMapGeneratorForDifficulty(int difficulty)
    {
        difficulty = Mathf.Clamp(difficulty, 0, sets.Length - 1);
        var set = sets[difficulty];
        var modules = new List<MapModuleSample>();
        var generator = new MapGenerator();


        foreach(var list in set.Standard)
        {
            modules.AddRange(list.MapModules);
        }
        generator.modules = modules.ToArray();
        modules.Clear();
        foreach (var list in set.Bottom)
        {
            modules.AddRange(list.MapModules);
        }
        generator.bottomModules = modules.ToArray();
        modules.Clear();
        foreach (var list in set.Top)
        {
            modules.AddRange(list.MapModules);
        }
        generator.topModules = modules.ToArray();
        modules.Clear();
        foreach (var list in set.Rare)
        {
            modules.AddRange(list.MapModules);
        }
        generator.rareModules = modules.ToArray();

        return generator;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            generator = GetMapGeneratorForDifficulty(GameManager.StageDifficulty);
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
                }

            }
            serializedArray = new SerializedMapModule[16];
            for(int i = 0; i < 16; i++)
            {
                serializedArray[i].index = MapModuleList.GetIndexFor(generatedMap[i].MapModuleSample);
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
            generatedMap[i] = new MapModule(MapModuleList.GetByIndex(serializedMap[i].index), serializedMap[i].flip);
        }
        MapPainter.Paint(generatedMap);
        generated = true;
    }
}