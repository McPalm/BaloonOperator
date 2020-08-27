using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponProperties Weapon;
    public SpriteRenderer SpriteRenderer;
         

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer.sprite = Weapon.sprite;
    }
    
}
