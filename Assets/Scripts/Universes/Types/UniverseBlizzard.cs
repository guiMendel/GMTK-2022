using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseBlizzard : UniverseType
{
    // === INTERFACE
    
    public ParticleSystem blizzard;


    // === STATE

    float blizzardDirection = 1f;


    // === REFS

    PlayerController playerController;

    
    protected override void OnStart() {
        playerController = FindObjectOfType<PlayerController>();

        EnsureNotNull.Objects(playerController);
    }

    protected override void OnActivate() {
        // Flip direction
        blizzardDirection = -blizzardDirection;
        
        // Flip the blizzard VFX
        blizzard.transform.localScale = new Vector3(
            blizzardDirection,
            blizzard.transform.localScale.y,
            blizzard.transform.localScale.z
        );
    }

    protected override void CounterbeatAction() {
        // Add a nudge to the player's movement
        playerController.rhythmicExecuter.AddBeatAction(
            "blizzardPush",
            playerController.movement.MakeMove(blizzardDirection, hop: false)
        );
    }
}
