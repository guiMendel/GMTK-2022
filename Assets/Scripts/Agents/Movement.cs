using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    // === INTERFACE
    
    public float jumpForce = 25f;

    // === STATE

    // Store movement speed, computed from cell distance and beat time
    float moveSpeed;

    // === REFS
    
    Rigidbody2D rigidBody;


    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();

        EnsureNotNull.Objects(rigidBody);

        // Calculate move speed
        Beat beat = FindObjectOfType<Beat>();
        Grid grid = FindObjectOfType<Grid>();
        
        float moveDistance = grid.cellSize.x;
        float moveTime = beat.secondsPerBeat / 2.0f;
        moveSpeed = moveDistance / moveTime;

        // Cancel horizontal movement on counterbeat
        FindObjectOfType<RhythmicExecuter>().OnIdleCounterbeat.AddListener(StopMovement);
    }

    private void OnDestroy() {
        // Clean up listener
        FindObjectOfType<RhythmicExecuter>()?.OnIdleCounterbeat.RemoveListener(StopMovement);

    }
    
    public UnityAction MakeJump(float powerModifier = 1.0f)
    {
        return () => rigidBody.AddForce(Vector2.up * jumpForce * powerModifier, ForceMode2D.Impulse);
    }

    public UnityAction MakeMove(float direction)
    {
        return () => rigidBody.velocity = new Vector2(direction * moveSpeed, rigidBody.velocity.y);
    }

    // Stops movement
    void StopMovement() {
        rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
    }
}
