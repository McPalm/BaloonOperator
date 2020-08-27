using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    public WeaponPickup WeaponPrefab;

    public List<WeaponProperties> weapons;

    // Start is called before the first frame update
    void Start()
    {
        int i = Random.Range(0, weapons.Count);
        FindObjectOfType<ItemManager>().Spawn(weapons[i], transform.position);
    }
}
