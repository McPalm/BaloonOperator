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

    private void OnEnable()
    {
        SpawnProjectile();
    }

    public void SpawnProjectile()
    {
        var spawned = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        var hurtThing = spawned.GetComponent<ContactDamage>();
        if(hurtThing)
            hurtThing.source = Source;
        var aim = spawned.GetComponent<TargetingMissile>();
        if (aim)
            aim.target = Source.GetComponent<EnemyAttack>()?.Target;

    }

}
