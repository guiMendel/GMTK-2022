using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlien : EnemyType
{
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseSpace>();

    protected override void BeatAction() {
        print("Pounce!");
    }
}
