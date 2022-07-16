using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYeti : EnemyType
{
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseBlizzard>();

    protected override void OnStart() {}

    protected override void BeatAction() {
        // print("Poop");
    }
}
