using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    public WeaponPickup WeaponPrefab;

    public List<LootTable> lootTables;


    // Start is called before the first frame update
    void Start()
    {
        if (MyNetworkManager.isServer)
        {
            int i = GameManager.StageDifficulty;
            i = Mathf.Clamp(i, 0, lootTables.Count-1);
            FindObjectOfType<ItemManager>().Spawn(lootTables[i].GetRandomLoot(), transform.position);
        }
    }
}
