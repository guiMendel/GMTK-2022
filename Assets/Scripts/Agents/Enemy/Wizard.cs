using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wizard : MonoBehaviour
{
    // === STATE 

    float moveSpeed;
    Vector3 initialPosition;
    bool freezePosition;

    public bool active;

    public UnityEvent OnReset;
    
    
    // === REFS

    PlayerController playerController;
    RhythmicExecuter rhythmicExecuter;
    Rigidbody2D rigidBody;


    private void Awake() {
        playerController = FindObjectOfType<PlayerController>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        rigidBody = GetComponent<Rigidbody2D>();
        OnReset ??= new UnityEvent();

        EnsureNotNull.Objects(playerController, rhythmicExecuter, rigidBody);
    }

    private void Start() {
        initialPosition = transform.position;
        playerController.OnRespawn.AddListener(ResetPosition);

        Grid grid = FindObjectOfType<Grid>();
        Beat beat = FindObjectOfType<Beat>();

        moveSpeed = grid.cellSize.x / (beat.SecondsPerCycle / 2.0f);
    }

    private void OnDestroy() {
        rhythmicExecuter.OnEveryBeat.RemoveListener(FollowPlayer);
        rhythmicExecuter.OnEveryCounterbeat.RemoveListener(StopMoving);
        playerController.OnRespawn.RemoveListener(ResetPosition);
    }

    public void BeginAction() {
        active = true;
        
        rhythmicExecuter.OnEveryBeat.AddListener(FollowPlayer);
        rhythmicExecuter.OnEveryCounterbeat.AddListener(StopMoving);
    }

    void FollowPlayer() {
        if (freezePosition) return;
        
        // Get distances
        float xDistance = playerController.transform.position.x - transform.position.x;
        float yDistance = playerController.transform.position.y - transform.position.y;

        // Flip if necessary
        if (Mathf.Sign(xDistance) != transform.localScale.x) {
            transform.localScale = new Vector3(
                Mathf.Sign(xDistance), transform.localScale.y, transform.localScale.z
            );
        }

        // Find player direction
        Vector2 moveDirection;

        if (Mathf.Abs(xDistance) >= Mathf.Abs(yDistance)){
            moveDirection = Vector2.right * Mathf.Sign(xDistance);
        }
        else {
            moveDirection = Vector2.up * Mathf.Sign(yDistance);
        }

        // Move
        rigidBody.velocity = moveDirection * moveSpeed;
    }

    void StopMoving() {
        rigidBody.velocity = Vector2.zero;
    }

    void ResetPosition() {
        OnReset.Invoke();
        active = false;
        
        transform.position = initialPosition;
        transform.localScale = Vector3.one;

        // Avoid moving this beat
        rigidBody.velocity = Vector2.zero;
        freezePosition = true;
        
        rhythmicExecuter.AddCounterbeatAction("restartMovement", () => freezePosition = false);
    }
}
