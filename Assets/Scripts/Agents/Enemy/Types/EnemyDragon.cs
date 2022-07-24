using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragon : EnemyType
{
    // === INTERFACE

    public float fireboxDelay = 0.3f;

    public ParticleSystem fireBreath;
    public Collider2D fireBox;
    public AudioSource sfx;

    protected override void OnStart() {
        // Disable firebox
        fireBox.enabled = false;        
    }

    
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseMedieval>();

    protected override void BeatAction() {
        // Start breathing fire
        fireBreath.Play();

        sfx.Play();

        // Activate firebox in a while
        StartCoroutine(DelayedFirebox());

        // Stop on downbeat
        rhythmicExecuter.AddDownbeatAction("ceaseFire", () => {
            fireBreath.Stop();
            fireBox.enabled = false;        
        });
    }

    IEnumerator DelayedFirebox() {
        // Wait delay
        yield return new WaitForSeconds(fireboxDelay);
        
        // Enable fire hitbox
        fireBox.enabled = true;        
    }
}
