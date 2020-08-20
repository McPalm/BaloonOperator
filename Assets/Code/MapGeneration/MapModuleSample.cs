using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Map Module", menuName = "Map Module", order = 11)]
public class MapModuleSample : ScriptableObject
{
    const int sizeX = 13;
    const int sizeY = 13;

    public bool OpenTop;
    public bool OpenBottom;
    public bool OpenBothSides;
    public MapModuleFlag MapModuleFlag;


    public TileBase[] tiles;

    public void CopyFrom(Tilemap tilemap, int x, int y)
    {
        tiles = tilemap.GetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(sizeX, sizeY, 1)));
    }

    public void PaintTo(Tilemap tilemap, int x, int y, bool flipped = false)
    {
        if (flipped)
            PaintFlipped(tilemap, x, y);
        else
            tilemap.SetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(sizeX, sizeY, 1)), tiles);
    }

    public void PaintFlipped(Tilemap tilemap, int startX, int startY)
    {
        var pos = new Vector3Int[tiles.Length];
        int i = 0;
        for(int y = startY; y < startY + sizeY; y++)
        {
            for(int x = startX + sizeX - 1 ; x >= startX; x--)
            {
                pos[i++] = new Vector3Int(x, y, 0);
            }
        }
        tilemap.SetTiles(pos, tiles);
    }    
}
