using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragon : EnemyType
{
    // === INTERFACE

    public ParticleSystem fireBreath;
    public Collider2D fireBox;

    protected override void OnStart() {
        // Disable firebox
        fireBox.enabled = false;        
    }

    
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseMedieval>();

    protected override void BeatAction() {
        // Start breathing fire
        fireBreath.Play();

        // Enable fire hitbox
        fireBox.enabled = true;        

        // Stop on counterbeat
        rhythmicExecuter.AddCounterbeatAction("ceaseFire", () => {
            fireBreath.Stop();
            fireBox.enabled = false;        
        });
    }
}
