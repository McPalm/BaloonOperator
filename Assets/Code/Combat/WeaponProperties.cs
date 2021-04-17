using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Properties", menuName = "Weapon Properties")]
public class WeaponProperties : ScriptableObject, LootTable.ILoot
{
    public Sprite sprite;
    public float lenght = .25f;
    public float attackSpeed = 1f;
    public int durability = 15;

    // moveset related
    public bool hammer;
    public bool oneHander;
    public bool bow;
    public bool spear;
    public bool slashing;
    public bool stabbing;

    // damage stats
    public bool breaksBlocks;
    public DamageProperties damageProperties;
}
