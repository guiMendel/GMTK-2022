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

    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();

        SnapToGrid();

        // Calculate move speed
        Beat beat = FindObjectOfType<Beat>();
        
        float moveDistance = grid.cellSize.x;
        float moveTime = beat.secondsPerBeat / 2.0f;
        moveSpeed = moveDistance / moveTime;
    }

    // Performs the current beat action
    public void PerformBeatAction() {
        // First, snap
        SnapToGrid();
        
        // Then, act

        // Check if need to move
        if (beatMoveDirection != 0) PerformMovement();

        // Jump
        if (beatJump) rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        else StandardAction();

        // Reset
        Reset();
    }

    // Action performed when no other action is selected
    private void StandardAction() {
        rigidBody.AddForce(Vector2.up * jumpForce / 3, ForceMode2D.Impulse);
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && callbackContext.ReadValueAsButton()) {
            // Add jump action
            beatJump = true;
        }
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        int inputDirection = (int)callbackContext.ReadValue<float>();

        // Ignore 0s
        beatMoveDirection = inputDirection == 0 ? beatMoveDirection : inputDirection;
    }

    public void Cancel(InputAction.CallbackContext callbackContext)
    {
        Reset();
    }

    void Reset()
    {
        beatMoveDirection = 0;
        beatJump = false;
    }

    // Stops movement
    public void StopMovement() {
        rigidBody.velocity = Vector2.zero;
    }

    void PerformMovement() {
        // Start moving in the correct direction
        rigidBody.velocity = Vector2.right * beatMoveDirection * moveSpeed;
    }

    // Snaps to grid position
    void SnapToGrid() {
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }



}
