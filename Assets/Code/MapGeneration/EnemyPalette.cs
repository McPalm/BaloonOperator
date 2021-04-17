using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPalette : MonoBehaviour
{
    
    public List<GameObject> EasyArtillery;
    public List<GameObject> HardArtillery;
    public List<GameObject> EasyGuardian;
    public List<GameObject> HardGuardian;

    List<GameObject> Palette;

    // Start is called before the first frame update
    void Start()
    {
        switch(GameManager.StageDifficulty)
        {
            case 0:
                Palette = PaletteFrom(EasyArtillery, EasyGuardian, EasyGuardian, EasyGuardian);
                break;
            case 1:
                Palette = PaletteFrom(EasyArtillery, EasyGuardian, HardGuardian, EasyGuardian);
                break;
            case 2:
                Palette = PaletteFrom(EasyArtillery, HardArtillery, HardGuardian, EasyGuardian);
                break;
            case 3:
                Palette = PaletteFrom(EasyArtillery, HardArtillery, HardGuardian, HardGuardian);
                break;
            default:
                Palette = PaletteFrom(HardArtillery, HardArtillery, HardGuardian, HardGuardian);
                break;
        }
    }

    public GameObject GetRandom() => Palette[Random.Range(0, Palette.Count)];

    List<GameObject> PaletteFrom(List<GameObject> a, List<GameObject> b, List<GameObject> c, List<GameObject> d)
    {
        var list = new List<GameObject>
        {
            a[Random.Range(0, a.Count)],
            b[Random.Range(0, b.Count)],
            c[Random.Range(0, c.Count)],
            d[Random.Range(0, d.Count)]
        };
        return list;
    }
}
