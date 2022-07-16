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

    public float groundCheckDistance = 0.6f;

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


    public void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();

        EnsureNotNull.Objects(rigidBody, grid);

        // Calculate move speed
        Beat beat = FindObjectOfType<Beat>();
        
        tileSize = grid.cellSize.x;
        moveTime = beat.secondsPerBeat / 2.0f;

        // Cancel horizontal movement on counterbeat
        FindObjectOfType<RhythmicExecuter>().OnEveryCounterbeat.AddListener(StopMovement);

        // Init direction
        Direction = 1f;
    }

    private void OnDestroy() {
        // Clean up listener
        FindObjectOfType<RhythmicExecuter>()?.OnEveryCounterbeat.RemoveListener(StopMovement);

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
        float direction, int tiles = 1, float obstructedMovementDelay = 0.0f, UnityAction callback = null
    )
    {
        Direction = Mathf.Sign(direction);

        // Rotate
        if (transform.localScale.x != Direction) FlipObject();

        return () => StartCoroutine(
            DelayMoveIfObstructed(Direction, obstructedMovementDelay, tiles, callback)
        );
    }

    void Move(float moveSpeed) {
        // Stop if not grounded
        if (IsGrounded == false) return;
        
        rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
    }

    IEnumerator DelayMoveIfObstructed(
        float direction, float delay, int tiles = 1, UnityAction callback = null
    ) {
        // Safeguard
        if (delay >= moveTime) {
            throw new Exception("Movement delay may not exceed movement time");
        }
        
        // Calculate move speed
        float effectiveDelay = IsObstructed() ? delay : 0.0f;
        float moveSpeed = Direction * tiles * tileSize / (moveTime - effectiveDelay);
        
        // Wait delay
        if (IsObstructed()) yield return new WaitForSeconds(delay);

        // Stop if not grounded
        if (IsGrounded == false) yield break;

        // Execute callback
        if (callback != null) callback();

        // Move
        Move(moveSpeed);
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

    public bool IsGrounded {
        get {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, -Vector2.up, groundCheckDistance, groundLayers
            );

            return hit.collider != null;
        }
    }
}
