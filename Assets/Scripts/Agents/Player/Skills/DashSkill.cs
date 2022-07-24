using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    // === INTERFACE

    public int tiles = 2;

    public ParticleSystem dashTrail;
    public AudioSource sfx;
    
    
    protected override void OnStart() {}

    protected override void DownbeatAction() {
        rhythmicExecuter.AddUpbeatAction("dash", () => {
            sfx.Play();
            movement.MakeMove(movement.Direction, tiles, hop: false)();
            dashTrail.Play();
        });
    }

}
