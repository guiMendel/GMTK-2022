using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDonkey : EnemyType
{
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseOldWest>();

    protected override void BeatAction() {
        print("Yee Haw!");
    }
}
