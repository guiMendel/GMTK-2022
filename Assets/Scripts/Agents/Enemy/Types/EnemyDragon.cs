using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragon : EnemyType
{
    // === INTERFACE

    public ParticleSystem fireBreath;

    
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseMedieval>();

    protected override void BeatAction() {
        // Start breathing fire
        fireBreath.Play();

        // Stop on counterbeat
        rhythmicExecuter.AddCounterbeatAction("ceaseFire", () => fireBreath.Stop());

        print(FindObjectOfType<TheDie>().Value);
    }
}
