using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlien : EnemyType
{
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseSpace>();

    protected override void OnStart() {}


    protected override void BeatAction() {
        // Pounce in the facing direction
        movement.MakeMove(movement.Direction, 3)();
    }
}
