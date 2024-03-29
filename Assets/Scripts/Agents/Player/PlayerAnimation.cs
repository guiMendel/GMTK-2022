using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AnimationManager
{
  // === ANIMATIONS

  const string LEAVE_GROUND = "Leave Ground";
  const string SPIN = "Spin";
  const string LAND = "Land";
  const string DIE = "Die";

  // === STATE

  // How many seconds after downbeat to start jump animation
  float jumpStartTimeAfterDownbeat;

  bool wasAirborne = false;

  // === REFS

  RhythmicExecuter rhythmicExecuter;
  Health health;
  SpriteRenderer spriteRenderer;

  override protected void OnStart()
  {
    rhythmicExecuter = GetComponent<RhythmicExecuter>();
    health = GetComponent<Health>();
    spriteRenderer = GetComponent<SpriteRenderer>();

    EnsureNotNull.Objects(rhythmicExecuter, health, spriteRenderer);

    GetJumpTime();

    // On downbeat, prepare to start jump animation
    rhythmicExecuter.OnEveryDownbeat.AddListener(CountJumpStart);

    // Subscribe to death
    health.OnDeath.AddListener(SetDie);
  }

  private void Update()
  {
    if (health.isDead) return;

    DetectAirborne();
  }

  private void OnDestroy()
  {
    rhythmicExecuter?.OnEveryDownbeat?.RemoveListener(CountJumpStart);
    health?.OnDeath?.RemoveListener(SetDie);
  }

  public void HideSprite()
  {
    spriteRenderer.enabled = false;
  }

  void DetectAirborne()
  {
    // Detect landing
    bool isAirborne = !movement.IsGrounded;

    // Detect landing
    if (wasAirborne && !isAirborne) SetAnimationState(LAND);

    // If is airborne, set spin unless is on Leave Ground
    if (isAirborne && currentState != LEAVE_GROUND)
    {
      SetAnimationState(SPIN);
    }

    wasAirborne = isAirborne;
  }

  void SetDie()
  {
    SetAnimationState(DIE);
  }

  void CountJumpStart()
  {
    if (health.isDead) return;

    StartCoroutine(JumpStartAfterDelay());
  }

  IEnumerator JumpStartAfterDelay()
  {
    // Wait until correct time
    yield return new WaitForSeconds(jumpStartTimeAfterDownbeat);

    // Stop if not grounded
    if (movement.IsGrounded == false) yield break;

    SetAnimationState(LEAVE_GROUND);
  }

  void GetJumpTime()
  {
    // Get jump animation duration
    float jumpDuration = Array.Find(
        animator.runtimeAnimatorController.animationClips,
        clip => clip.name == LEAVE_GROUND
    ).length;

    // Get half the beat's time
    float halfBeat = FindObjectOfType<Beat>().SecondsPerCycle / 2.0f;

    // Get the duration of the anticipation (happens at the 5th frame, of the 7)
    float jumpAnticipationDuration = jumpDuration / Time.timeScale * 5 / 7;

    // Calculate the time
    jumpStartTimeAfterDownbeat = halfBeat - jumpAnticipationDuration;

    if (jumpAnticipationDuration < 0)
    {
      throw new Exception("Player jump anticipation starts before the downbeat");
    }
  }

}
