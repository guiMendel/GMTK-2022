using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    // === INTERFACE
    
    public float jumpForce = 25f;

    public LayerMask groundLayers;

    public float groundCheckDistance = 0.2f;

    [Range(0f, 1f)] public float hopPowerFraction = 0.35f;

    [Range(0f, 1f)] public float hopGravityReduction = 0.35f;

    // === STATE

    // Store 1 square movement distance
    float tileSize;

    // Store movement time
    float moveTime;

    // Remember last movement direction
    public float Direction { get; set; }

    // === REFS
    
    Rigidbody2D rigidBody;
    Grid grid;
    RhythmicExecuter rhythmicExecuter;
    Collider2D collider2d;


    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        collider2d = GetComponent<Collider2D>();

        EnsureNotNull.Objects(rigidBody, grid, rhythmicExecuter, collider2d);

        // Calculate move speed
        Beat beat = FindObjectOfType<Beat>();
        
        tileSize = grid.cellSize.x;
        moveTime = beat.SecondsPerCycle / 2.0f;

        // Cancel horizontal movement on downbeat
        rhythmicExecuter.OnEveryDownbeat.AddListener(StopMovement);

        // Hop when idle
        rhythmicExecuter.OnIdleUpbeat.AddListener(Hop);

        // Init direction
        Direction = 1f;
    }

    private void OnDestroy() {
        // Clean up listener
        rhythmicExecuter?.OnEveryDownbeat.RemoveListener(StopMovement);

    }

    // Checks if the cell right ahead is occupied by terrain
    public bool IsObstructed() {
        Tilemap tilemap = FindObjectOfType<Tilemap>();
        
        Vector3Int nextCell = grid.WorldToCell(transform.position) + Vector3Int.right * (int)Direction;
        
        return tilemap.HasTile(nextCell);
    }
    
    public UnityAction MakeJump(float powerModifier = 1.0f)
    {
        return () => rigidBody.AddForce(Vector2.up * jumpForce * powerModifier, ForceMode2D.Impulse);
    }

    public UnityAction MakeMove(
        float direction,
        int tiles = 1,
        float obstructedMovementDelay = 0.0f,
        UnityAction callback = null,
        bool hop = true
    )
    {
        Direction = Mathf.Sign(direction);

        // Rotate
        if (transform.localScale.x != Direction) FlipObject();

        return () => StartCoroutine(
            DelayMoveIfObstructed(Mathf.Sign(direction), obstructedMovementDelay, tiles, callback, hop)
        );
    }

    void Move(float moveSpeed, bool hop) {
        // Also hop
        if (hop) Hop();
        
        rigidBody.velocity = new Vector2(rigidBody.velocity.x + moveSpeed, rigidBody.velocity.y);

        // Update direction based on resulting speed
        Direction = Mathf.Sign(rigidBody.velocity.x);
    }

    IEnumerator DelayMoveIfObstructed(
        float direction, float delay, int tiles = 1, UnityAction callback = null, bool hop = true
    ) {
        // Safeguard
        if (delay >= moveTime) {
            throw new Exception("Movement delay may not exceed movement time");
        }
        
        // Stop if not grounded
        if (IsGrounded == false) yield break;

        // Calculate move speed
        float effectiveDelay = IsObstructed() ? delay : 0.0f;
        float moveSpeed = direction * tiles * tileSize / (moveTime - effectiveDelay);

        // Wait delay
        if (IsObstructed()) yield return new WaitForSeconds(delay);

        // Execute callback
        if (callback != null) callback();

        // Move
        Move(moveSpeed, hop);
    }

    void FlipObject() {
        // Change local scale
        transform.localScale = new Vector3(
            Direction, transform.localScale.y, transform.localScale.z
        );

        // Change particle systems flip state
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem system in systems) {
            system.transform.localScale = new Vector3(
                Direction, system.transform.localScale.y, system.transform.localScale.z
            );
        }

    }

    // Stops movement
    void StopMovement() {
        rigidBody.velocity = new Vector2(0.0f, rigidBody.velocity.y);
    }

    void Hop() {
        // Stop if not grounded or has jump
        if (
            IsGrounded == false || rhythmicExecuter.GetUpbeatAction("jump") != null
        ) return;
        
        ReduceGravity();

        MakeJump(hopPowerFraction)();
    }

    public bool IsGrounded {
        get {
            // RaycastHit2D[] hits = new RaycastHit2D[1];
            int hitCount = collider2d.Cast(Vector2.down, new RaycastHit2D[1], groundCheckDistance);

            return hitCount > 0;
        }
    }

    public void ReduceGravity() {
        rigidBody.gravityScale = hopGravityReduction;

        // Return to normal on downbeat
        rhythmicExecuter.AddDownbeatAction(
            "returnGravityScale", () => rigidBody.gravityScale = 1f
        );
    }
}
