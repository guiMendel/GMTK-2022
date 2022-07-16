using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // === INTERFACE

    public float jumpMoveDelay = 0.1f;

    // === STATE 

    // Remember starting position
    Vector3 startingPosition;
    
    // === REFS
    
    RhythmicExecuter rhythmicExecuter;
    Movement movement;

    public void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();

        EnsureNotNull.Objects(rhythmicExecuter, movement);

        startingPosition = transform.position;
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

        // If already had a move action, double it's speed
        if (rhythmicExecuter.GetBeatAction("move") == null) return;

        // If obstructed, delay move
        MoveDelayIfObstructed(movement.Direction, 2);

    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        float inputDirection = callbackContext.ReadValue<float>();

        // Ignore 0s
        if (inputDirection == 0) return;

        // If has a jump action, move double the distance
        int tiles = rhythmicExecuter.GetBeatAction("jump") != null ? 2 : 1;
        
        // If is obstructed, try delaying movement so that jump cna hop over single tile height walls
        MoveDelayIfObstructed(inputDirection, tiles);
    }

    void MoveDelayIfObstructed(float direction, int tiles) {
        if (movement.IsObstructed()) {
            rhythmicExecuter.AddBeatAction(
                "move", movement.MakeDelayedMove(direction, jumpMoveDelay, tiles)
            );

            return;
        }

        rhythmicExecuter.AddBeatAction("move", movement.MakeMove(direction, tiles));
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

        // Respawn on beat
        rhythmicExecuter.AddBeatAction("respawn", () => {
            // Return to starting position and reenable
            transform.position = startingPosition;

            spriteRenderer.enabled = true;
            rigidBody.isKinematic = false;
            rigidBody.velocity = Vector3.zero;
        });
    }
}
