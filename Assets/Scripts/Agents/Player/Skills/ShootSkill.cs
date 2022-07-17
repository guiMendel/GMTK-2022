using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkill : Skill
{
    // === INTERFACE

    public GameObject projectile;
    public AudioSource sfx;

    
    protected override void OnStart() {}

    protected override void BeatAction() {
        Instantiate(projectile, transform.position, Quaternion.identity);

        sfx.Play();

        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();

        projectileMovement.direction = movement.Direction;
    }
}
