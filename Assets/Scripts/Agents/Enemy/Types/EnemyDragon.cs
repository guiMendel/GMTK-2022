using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDragon : EnemyType
{
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseMedieval>();

    protected override void BeatAction() {
        print("FIRE BREATH!");
    }
}
