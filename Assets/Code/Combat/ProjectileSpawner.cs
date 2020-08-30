using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spawns a porjectile whine enabled. network sync is only managed by whoever enables or disables the script.
/// </summary>
public class ProjectileSpawner : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject Source;
    public DamageProperties damage;

    public bool ManuallyAim;

    private void OnEnable()
    {
        SpawnProjectile();
    }


    public void SpawnProjectile()
    {
        var spawned = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        var hurtThing = spawned.GetComponent<ContactDamage>();
        if(hurtThing)
        {
            hurtThing.source = Source;
            if (damage.damage > -1)
                hurtThing.damagePropeties = damage;
        }

        var aim = spawned.GetComponent<TargetingMissile>();
        if (aim)
            aim.target = Source.GetComponent<EnemyAttack>()?.Target;
        if (ManuallyAim)
        {
            spawned.transform.right = Source.GetComponent<EnemyAttack>().GetAimPosition() - transform.position;
        }
    }

}
