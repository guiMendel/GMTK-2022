using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    // === INTERFACE
    
    public float jumpForce = 20f;
    
    // === STATE

    // Which direction to move on beat
    int beatMoveDirection;

    // Whether to jump on beat
    bool beatJump;

    // Store movement speed, computed from cell distance and beat time
    float moveSpeed;

    // === REFS
    
    Rigidbody2D rigidBody;
    Grid grid;
    RhythmicExecuter rhythmicExecuter;

    public void Start() {
        grid = FindObjectOfType<Grid>();
        rigidBody = GetComponent<Rigidbody2D>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();

        // Calculate move speed
        Beat beat = FindObjectOfType<Beat>();
        
        float moveDistance = grid.cellSize.x;
        float moveTime = beat.secondsPerBeat / 2.0f;
        moveSpeed = moveDistance / moveTime;

        // Set default beat action to small skip
        rhythmicExecuter.defaultBeatAction = StandardAction;


        // Set default counterbeat action to stop movement
        rhythmicExecuter.defaultCounterbeatAction = StopMovement;
    }

    // Action performed when no other action is selected
    private void StandardAction() {
        rigidBody.AddForce(Vector2.up * jumpForce / 3, ForceMode2D.Impulse);
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && callbackContext.ReadValueAsButton()) {
            // Add jump action
            rhythmicExecuter.AddBeatAction(
                "jump",
                () => rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse)
            );
        }
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        int inputDirection = (int)callbackContext.ReadValue<float>();

        // Ignore 0s
        if (inputDirection == 0) return;

        rhythmicExecuter.AddBeatAction(
            "move",
            () => rigidBody.velocity = new Vector2(inputDirection * moveSpeed, rigidBody.velocity.y)
        );
    }

    public void Cancel(InputAction.CallbackContext callbackContext)
    {
        rhythmicExecuter.ClearAll();
    }

    // Stops movement
    public void StopMovement() {
        rigidBody.velocity = Vector2.zero;
    }
}
