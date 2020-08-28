using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public List<WeaponProperties> CommonWeapons;
    public List<WeaponProperties> UncommonWeapons;

    public ILoot GetRandomLoot()
    {
        if (Random.value < .2f)
            return UncommonWeapons[Random.Range(0, UncommonWeapons.Count)];
        return CommonWeapons[Random.Range(0, CommonWeapons.Count)];
    }

    public interface ILoot
    { }
}
