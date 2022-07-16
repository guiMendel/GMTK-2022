using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // === REFS
    
    RhythmicExecuter rhythmicExecuter;
    Movement movement;

    public void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();

        EnsureNotNull.Objects(rhythmicExecuter, movement);
    }

    // Action performed when no other action is selected
    public void StandardAction() {
        movement.MakeJump(0.3f)();
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed || !callbackContext.ReadValueAsButton()) return;

        // Add jump action
        rhythmicExecuter.AddBeatAction("jump", movement.MakeJump());
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        float inputDirection = callbackContext.ReadValue<float>();

        // Ignore 0s
        if (inputDirection == 0) return;

        rhythmicExecuter.AddBeatAction("move", movement.MakeMove(inputDirection));
    }

    public void Cancel(InputAction.CallbackContext callbackContext)
    {
        rhythmicExecuter.ClearAll();
    }

    public void Die() {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine(float resetDelay = 1.5f) {
        // Hide & disable physics
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        spriteRenderer.enabled = false;
        rigidBody.velocity = Vector3.zero;
        rigidBody.isKinematic = true;
        
        // Wait time
        yield return new WaitForSeconds(resetDelay);
        
        StageController stageController = FindObjectOfType<StageController>();

        stageController.ResetStage();
    }
}
