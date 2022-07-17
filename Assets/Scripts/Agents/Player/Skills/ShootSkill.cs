using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkill : Skill
{
    // === INTERFACE

    public GameObject projectile;

    
    protected override void OnStart() {}

    protected override void OnActivate() {
        Instantiate(projectile, transform.position, Quaternion.identity);

        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();

        projectileMovement.direction = movement.Direction;
    }
}
