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

    public UnityEvent OnRespawn;
    public SpriteRenderer shieldRenderer;
    public AudioSource dieSfx;

    // === STATE 

    // Remember starting position
    Vector3 startingPosition;

    float holdingMovementKey = 0f;
    
    // === REFS
    
    public RhythmicExecuter rhythmicExecuter;
    public Movement movement;
    Collider2D collider2d;
    Grid grid;
    Tilemap tilemap;
    Rigidbody2D body;
    Health health;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();
        collider2d = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        grid = FindObjectOfType<Grid>();
        tilemap = FindObjectOfType<Tilemap>();
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        EnsureNotNull.Objects(rhythmicExecuter, movement, collider2d, grid, tilemap, body, health, spriteRenderer);

        OnRespawn ??= new UnityEvent();
    }

    public void Start() {
        startingPosition = transform.position;

        health.OnDeath.AddListener(Die);

        rhythmicExecuter.OnEveryDownbeat.AddListener(AddMoveIfHolding);
    }

    private void OnDestroy() {
        health.OnDeath.RemoveListener(Die);
        rhythmicExecuter.OnEveryDownbeat.RemoveListener(AddMoveIfHolding);
    }
    
    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.performed || !callbackContext.ReadValueAsButton()) return;

        // Add jump action
        rhythmicExecuter.AddUpbeatAction("jump", movement.MakeJump());

        // If already had a move action, double it's speed
        if (rhythmicExecuter.GetUpbeatAction("move") == null) return;

        rhythmicExecuter.AddUpbeatAction(
            "move", movement.MakeMove(movement.Direction, 2, jumpMoveDelay)
        );
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        holdingMovementKey = callbackContext.ReadValue<float>();

        // Ignore 0s
        if (holdingMovementKey == 0) return;

        Move(holdingMovementKey);
    }

    void Move(float direction) {
        // If has a jump action, move double the distance
        int tiles = rhythmicExecuter.GetUpbeatAction("jump") != null ? 2 : 1;
        
        rhythmicExecuter.AddUpbeatAction(
            "move", movement.MakeMove(direction, tiles, jumpMoveDelay)
        );
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
        body.velocity = Vector3.zero;
        body.isKinematic = true;
        collider2d.enabled = false;
        shieldRenderer.enabled = false;

        dieSfx.Play();
        
        // Wait time
        yield return new WaitForSeconds(resetDelay);

        // Respawn on beat
        rhythmicExecuter.AddUpbeatAction("respawn", Respawn);
    }

    void Respawn() {
        OnRespawn.Invoke();

        // Return to starting position and reenable
        transform.position = startingPosition;

        spriteRenderer.enabled = true;
        body.isKinematic = false;
        collider2d.enabled = true;
        body.velocity = Vector3.zero;
        health.isDead = false;
        shieldRenderer.enabled = true;
    }

    void AddMoveIfHolding() {
        if (holdingMovementKey == 0f) return;

        Move(holdingMovementKey);
    }
}
