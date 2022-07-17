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

    // === STATE

    // How many seconds after counterbeat to start jump animation
    float jumpStartTimeAfterCounterbeat;

    bool wasAirborne = false;

    // === REFS

    RhythmicExecuter rhythmicExecuter;
    
    override protected void OnStart() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(rhythmicExecuter);

        GetJumpTime();
        
        // On counterbeat, prepare to start jump animation
        rhythmicExecuter.OnEveryCounterbeat.AddListener(CountJumpStart);
    }

    private void Update() {
        // Detect landing
        bool isAirborne = !movement.IsGrounded;

        // Detect landing
        if (wasAirborne && !isAirborne) SetAnimationState(LAND);

        // If is airborne, set spin unless is on Leave Ground
        if (isAirborne && currentState != LEAVE_GROUND) {
            SetAnimationState(SPIN);
        }

        wasAirborne = isAirborne;
    }

    private void OnDestroy() {
        rhythmicExecuter?.OnEveryCounterbeat?.RemoveListener(CountJumpStart);
    }

    void CountJumpStart() {
        StartCoroutine(JumpStartAfterDelay());
    }

    IEnumerator JumpStartAfterDelay() {
        // Wait until correct time
        yield return new WaitForSeconds(jumpStartTimeAfterCounterbeat);

        // Stop if not grounded
        if (movement.IsGrounded == false) yield break;

        SetAnimationState(LEAVE_GROUND);
    }

    void GetJumpTime() {
        // Get jump animation duration
        float jumpDuration = Array.Find(
            animator.runtimeAnimatorController.animationClips,
            clip => clip.name == LEAVE_GROUND
        ).length;

        // Get half the beat's time
        float halfBeat = FindObjectOfType<Beat>().secondsPerBeat / 2.0f;

        // Get the duration of the anticipation (happens at the 6th frame, of the 7)
        float jumpAnticipationDuration = jumpDuration * 6/7;

        // Calculate the time
        jumpStartTimeAfterCounterbeat = halfBeat - jumpAnticipationDuration;

        if (jumpAnticipationDuration < 0) {
            throw new Exception("Player jump anticipation starts before the counterbeat");
        }
    }

}
