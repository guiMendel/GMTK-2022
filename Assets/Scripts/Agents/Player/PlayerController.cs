using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    // === INTERFACE

    public float jumpMoveDelay = 0.1f;

    [Range(0f, 1f)] public float skipPowerFraction = 0.5f;

    public float hopGravityReduction = 0.7f;

    // === STATE 

    // Remember starting position
    Vector3 startingPosition;
    
    // === REFS
    
    RhythmicExecuter rhythmicExecuter;
    Movement movement;
    Collider2D collider2d;
    Grid grid;
    Tilemap tilemap;
    Rigidbody2D body;

    public void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();
        collider2d = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();
        tilemap = FindObjectOfType<Tilemap>();

        EnsureNotNull.Objects(rhythmicExecuter, movement, collider2d, grid, tilemap, GetComponent<Rigidbody>());

        startingPosition = transform.position;
    }

    // Action performed when no other action is selected
    public void StandardAction() {
        movement.MakeJump(skipPowerFraction)();
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed || !callbackContext.ReadValueAsButton()) return;

        // Add jump action
        rhythmicExecuter.AddBeatAction("jump", movement.MakeJump(1f - skipPowerFraction));

        // If already had a move action, double it's speed
        if (rhythmicExecuter.GetBeatAction("move") == null) return;

        rhythmicExecuter.AddBeatAction(
            "move", movement.MakeMove(movement.Direction, 2, jumpMoveDelay)
        );
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        float inputDirection = callbackContext.ReadValue<float>();

        // Ignore 0s
        if (inputDirection == 0) return;

        // If has a jump action, move double the distance
        int tiles = rhythmicExecuter.GetBeatAction("jump") != null ? 2 : 1;

        // Only reduce gravity if no jumping
        UnityAction callback = rhythmicExecuter.GetBeatAction("jump") == null
            ? ReduceGravity
            : null;
        
        rhythmicExecuter.AddBeatAction(
            "move", movement.MakeMove(inputDirection, tiles, jumpMoveDelay, callback)
        );
    }

    public void Cancel(InputAction.CallbackContext callbackContext)
    {
        rhythmicExecuter.ClearAll();
    }

    public void Die() {
        StartCoroutine(DieCoroutine());
    }

    public void ReduceGravity() {
        body.gravityScale = hopGravityReduction;

        // Return to normal on counterbeat
        rhythmicExecuter.AddCounterbeatAction(
            "returnGravityScale", () => body.gravityScale = 1f
        );
    }

    IEnumerator DieCoroutine(float resetDelay = 1.5f) {
        // Hide & disable physics
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        collider2d.enabled = false;
        
        // Wait time
        yield return new WaitForSeconds(resetDelay);

        // Respawn on beat
        rhythmicExecuter.AddBeatAction("respawn", () => {
            // Return to starting position and reenable
            transform.position = startingPosition;

            spriteRenderer.enabled = true;
            body.isKinematic = false;
            collider2d.enabled = true;
            body.velocity = Vector3.zero;
        });
    }
}
