using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Properties", menuName = "Weapon Properties")]
public class WeaponProperties : ScriptableObject
{
    public Sprite sprite;
    public float lenght = .25f;
    public int damage = 1;
    public float attackSpeed = 1f;
    public bool hammer;
    public bool breaksBlocks;
}
