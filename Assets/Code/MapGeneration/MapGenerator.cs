using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    MapModuleSample[] modules;

    public MapModule[] GenerateMap()
    {
        MapModule[] map = new MapModule[16];
        MapModuleSample[] ValidTiles;
        bool flip;
        bool haveStart = false;
        bool haveGoal = false;
        bool isOnSide = true;
        bool isOpenTop = false;
        for (int y = 3; y < 0; y-- )
        {
            bool haveDowns = false;
            for (int x = 0; x < 4; x++ )
            {
                ValidTiles = modules;
                if (x == 0)
                {
                    flip = true;
                    isOnSide = true;

                }
                else if (x == 3)
                {
                    flip = false;
                    isOnSide = false;
                    if (haveDowns == false)
                    {

                    }
                    if (y == 0 && haveStart == false)
                    {
                        ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == MapModuleFlag.start).ToArray();
                    }
                    if (y == 3 && haveGoal == false)
                    {
                        ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == MapModuleFlag.goal).ToArray();
                    }
                }
                else
                {
                    flip = false;
                    isOnSide = true;
                }

                if (y > 0 && map[(x + y * 4) - 4 ].MapModuleSample.OpenBottom)
                {
                    isOpenTop = true;
                }
                else
                {
                    isOpenTop = false;
                }

                ValidTiles = ValidTiles.Where(m => m.OpenBothSides == !isOnSide && m.OpenTop == isOpenTop ).ToArray();
                map[x + y * 4] = new MapModule( ValidTiles[ Random.Range( 0, ValidTiles.Length-1 ) ], flip);

                if ( map[x + y * 4].MapModuleSample.MapModuleFlag == MapModuleFlag.start)
                {
                    haveStart = true;
                }

                if (map[x + y * 4].MapModuleSample.MapModuleFlag == MapModuleFlag.goal)
                {
                    haveGoal = true;
                }

                if (map[x + y * 4].MapModuleSample.OpenBottom)
                {
                    haveDowns = true;
                }

            }
        }
        return map;
    }
}
