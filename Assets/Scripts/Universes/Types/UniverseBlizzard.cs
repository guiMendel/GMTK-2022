using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseBlizzard : UniverseType
{
  // === INTERFACE

  public ParticleSystem blizzard;

  public bool stormActive = true;


  // === STATE

  float blizzardDirection = 1f;


  // === REFS

  PlayerController playerController;


  protected override void OnAwake()
  {
    playerController = FindObjectOfType<PlayerController>();

    EnsureNotNull.Objects(playerController);
  }

  protected override void OnStart() { }

  protected override void OnActivate()
  {
    // Activate or not the vfx
    if (stormActive) blizzard.Play();
    else blizzard.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

    // Flip direction
    blizzardDirection = -blizzardDirection;

    // Flip the blizzard VFX
    blizzard.transform.localScale = new Vector3(
        blizzardDirection,
        blizzard.transform.localScale.y,
        blizzard.transform.localScale.z
    );
  }

  protected override void DownbeatAction()
  {
    if (stormActive == false) return;

    // Add a nudge to the player's movement
    playerController.rhythmicExecuter.AddUpbeatAction(
        "blizzardPush",
        playerController.movement.MakeMove(blizzardDirection, hop: false)
    );
  }
}
