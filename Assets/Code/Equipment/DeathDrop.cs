using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathDrop : MonoBehaviour
{
    public LootTable LootTable;

    // Start is called before the first frame update
    void Start()
    {
        var health = GetComponent<Health>();
        health.OnZeroHealth += Health_OnZeroHealth;
    }

    private void Health_OnZeroHealth()
    {
        var weapon = LootTable.GetRandomLoot() as WeaponProperties;
        int durabilityLoss = Random.Range(0, weapon.durability / 4 * 3);
        FindObjectOfType<ItemManager>().Spawn(weapon, transform.position, durabilityLoss);
    }
}
