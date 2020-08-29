using UnityEngine;
using System.Linq;

public class MapGenerator
{
    public MapModuleSample[] modules;
    public MapModuleSample[] rareModules;
    public MapModuleSample[] topModules;
    public MapModuleSample[] bottomModules;

    int IndexFor(int x, int y) => x + y * 4;

    public MapModule[] GenerateMap()
    {
        MapModule[] map = new MapModule[16];
        MapModuleSample[] ValidTiles;
        bool haveStart = false;
        bool haveGoal = false;
        bool haveChallenge = false;
        bool haveMonster = false;
        bool haveRare = false;

        for (int y = 3; y >= 0; y-- )
        {
            bool haveDowns = false;
            for (int x = 0; x < 4; x++ )
            {
                var isSide = x == 0 || x == 3;
                var flip = x == 0;
                var isOpenTop = y < 3 && map[IndexFor(x, y+1)].MapModuleSample.OpenBottom;
                ValidTiles = modules;
                if(y == 3)
                    ValidTiles = ValidTiles.Concat(topModules).ToArray();
                if(y == 0)
                    ValidTiles = ValidTiles.Concat(bottomModules).ToArray();


                MapModuleFlag desiredFlag = MapModuleFlag.none;
                switch (y)
                {
                    case 3:
                        if (!haveStart)
                        {
                            if (Random.value < (x + 1) / 4f)
                            {
                                desiredFlag = MapModuleFlag.start;
                            }
                        }
                        break;
                    case 1:
                    case 2:
                        int odds = 0;
                        odds += !haveMonster ? 1 : 0;
                        odds += !haveChallenge ? 1 : 0;
                        int roomsLeft = y * 4 - x - 2;
                        if(odds > 0 && Random.Range(0, odds) >= Random.Range(0,roomsLeft))
                        {
                            if (haveChallenge)
                                desiredFlag = MapModuleFlag.monster;
                            else if (haveMonster)
                                desiredFlag = MapModuleFlag.challenge;
                            else
                                desiredFlag = Random.value < .5 ? MapModuleFlag.monster : MapModuleFlag.challenge;
                        }
                        break;
                    case 0:
                        if (!haveGoal && y == 0)
                        {
                            if (Random.value < (x + 1) / 4f)
                                desiredFlag = MapModuleFlag.goal;
                        }
                        else if (!haveMonster || !haveChallenge)
                        {
                            if (haveChallenge)
                                desiredFlag = MapModuleFlag.monster;
                            else if (haveMonster)
                                desiredFlag = MapModuleFlag.challenge;
                            else
                                desiredFlag = Random.value < .5 ? MapModuleFlag.monster : MapModuleFlag.challenge;
                        }
                        break;
                }

                ValidTiles = ValidTiles.Where(m => m.MapModuleFlag == desiredFlag).ToArray();
                if (desiredFlag == MapModuleFlag.none && !haveRare && rareModules.Length > 0 && Random.value * (1+y) < .2f)
                {
                    ValidTiles = rareModules;
                    haveRare = true;
                }

                if (x == 3)
                {
                    if(!haveDowns && y != 0)
                    {
                        ValidTiles = ValidTiles.Where(m => m.OpenBottom == true).ToArray();
                    }
                }
                if(y == 0)
                {
                    ValidTiles = ValidTiles.Where(m => m.OpenBottom == false).ToArray();
                }

                ValidTiles = ValidTiles.Where(m => m.OpenBothSides == !isSide && m.OpenTop == isOpenTop ).ToArray();
                map[IndexFor(x, y)] = new MapModule( ValidTiles[ Random.Range( 0, ValidTiles.Length) ], flip);

                if (map[IndexFor(x, y)].MapModuleSample.OpenBothSides == true)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        map[IndexFor(x, y)].flip = true;
                    }
                }
                switch(map[IndexFor(x, y)].MapModuleSample.MapModuleFlag)
                {
                    case MapModuleFlag.start:
                        haveStart = true;
                        break;
                    case MapModuleFlag.goal:
                        haveGoal = true;
                        break;
                    case MapModuleFlag.challenge:
                        haveChallenge = true;
                        break;
                    case MapModuleFlag.monster:
                        haveMonster = true;
                        break;
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
