using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    // === INTERFACE

    public int tiles = 2;

    public ParticleSystem dashTrail;
    
    
    protected override void OnStart() {}

    protected override void CounterbeatAction() {
        rhythmicExecuter.AddBeatAction("dash", () => {
            movement.MakeMove(movement.Direction, tiles, hop: false)();
            dashTrail.Play();
        });
    }

}
