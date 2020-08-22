using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Map Module", menuName = "Map Module", order = 11)]
public class MapModuleSample : ScriptableObject
{
    const int sizeX = 17;
    const int sizeY = 13;

    public bool OpenTop;
    public bool OpenBottom;
    public bool OpenBothSides;
    public MapModuleFlag MapModuleFlag;


    public TileBase[] tiles;

    public void CopyFrom(Tilemap tilemap, int x, int y, bool flipped = false)
    {
        if(flipped)
            tiles = ToFlipped(tilemap.GetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(sizeX, sizeY, 1))));
        else
            tiles = tilemap.GetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(sizeX, sizeY, 1)));
    }

    public void PaintTo(Tilemap tilemap, int x, int y, bool flipped = false)
    {
        if (flipped)
            tilemap.SetTilesBlock(new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(sizeX, sizeY, 1)), ToFlipped(tiles));
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

    static public TileBase[] ToFlipped(TileBase[] from)
    {
        var to = new TileBase[from.Length];

        for(int i = 0; i < to.Length; i++)
        {
            to[i] = from[sizeX - 1 - i % sizeX + (i / sizeX) * sizeX];
        }

        return to;
    }
}
