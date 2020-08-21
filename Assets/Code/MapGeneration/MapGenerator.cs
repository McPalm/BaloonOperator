using UnityEngine;
using System.Linq;

public class MapGenerator
{
    public MapModuleSample[] modules;

    int IndexFor(int x, int y) => x + y * 4;

    public MapModule[] GenerateMap()
    {
        MapModule[] map = new MapModule[16];
        MapModuleSample[] ValidTiles;
        bool haveStart = false;
        bool haveGoal = false;
        for (int y = 3; y >= 0; y-- )
        {
            bool haveDowns = false;
            for (int x = 0; x < 4; x++ )
            {
                var isSide = x == 0 || x == 3;
                var flip = x == 0;
                var isOpenTop = y < 3 && map[IndexFor(x, y+1)].MapModuleSample.OpenBottom;
                ValidTiles = modules;


                if (x == 3)
                {
                    if (y == 3 && haveStart == false)
                    {
                        ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == MapModuleFlag.start).ToArray();
                    }
                    if (y == 0 && haveGoal == false)
                    {
                        ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == MapModuleFlag.goal).ToArray();
                    }
                    if(!haveDowns)
                    {
                        ValidTiles = ValidTiles.Where(m => m.OpenBottom == true).ToArray();
                    }
                }
                if(y == 0)
                    ValidTiles = ValidTiles.Where(m => m.MapModuleFlag != MapModuleFlag.start).ToArray();
                if (y == 1 || y == 2)
                    ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == MapModuleFlag.none).ToArray();
                if(y == 3)
                    ValidTiles = ValidTiles.Where(m => m.MapModuleFlag != MapModuleFlag.goal).ToArray();


                ValidTiles = ValidTiles.Where(m => m.OpenBothSides == !isSide && m.OpenTop == isOpenTop ).ToArray();
                map[IndexFor(x, y)] = new MapModule( ValidTiles[ Random.Range( 0, ValidTiles.Length) ], flip);

                if ( map[IndexFor(x, y)].MapModuleSample.MapModuleFlag == MapModuleFlag.start)
                {
                    haveStart = true;
                }

                if (map[IndexFor(x, y)].MapModuleSample.MapModuleFlag == MapModuleFlag.goal)
                {
                    haveGoal = true;
                }

                if (map[IndexFor(x, y)].MapModuleSample.OpenBottom)
                {
                    haveDowns = true;
                }

            }
        }
        return map;
    }
}
