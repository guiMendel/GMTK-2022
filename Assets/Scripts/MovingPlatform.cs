using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // === INTERFACE

    public int remainTimeBeats = 0;

    // Waypoints container that should hold the waypoints to follow
    public Transform WaypointsContainer;

    public float objectSnapDistance = 0.3f;

    // === STATE 

    // Current waypoint to follow
    int waypointIndex = 0;

    float moveSpeed;

    int remainingBeatsStill;

    // === REFS

    RhythmicExecuter rhythmicExecuter;
    Grid grid;
    Rigidbody2D rigidBody;
    Collider2D collider2d;

    private void Start() {
        rhythmicExecuter = FindObjectOfType<RhythmicExecuter>();
        grid = FindObjectOfType<Grid>();
        rigidBody = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        EnsureNotNull.Objects(rhythmicExecuter, grid, rigidBody);

        // Set default beat action to follow waypoint
        rhythmicExecuter.OnEveryBeat.AddListener(MoveToWaypoint);
        rhythmicExecuter.OnEveryCounterbeat.AddListener(StopMoving);

        Beat beat = FindObjectOfType<Beat>();

        moveSpeed = grid.cellSize.x / (beat.SecondsPerCycle / 2.0f);
    }

    private void OnDestroy() {
        // Clean up listeners
        rhythmicExecuter?.OnEveryBeat?.RemoveListener(MoveToWaypoint);
        rhythmicExecuter?.OnEveryCounterbeat?.RemoveListener(StopMoving);
    }

    void MoveToWaypoint() {
        if (remainingBeatsStill > 0) {
            remainingBeatsStill--;
            return;
        }
        
        // Get waypoint
        Transform waypoint = WaypointsContainer.GetChild(waypointIndex);

        // Detect arrival
        if (grid.WorldToCell(transform.position) == grid.WorldToCell(waypoint.position)) {
            // Advance waypoint
            waypointIndex = (waypointIndex + 1) % WaypointsContainer.childCount;

            waypoint = WaypointsContainer.GetChild(waypointIndex);

            // Check if need to stay still
            if (remainTimeBeats > 0) {
                remainingBeatsStill = remainTimeBeats - 1;
                return;
            }
        }

        // Move towards it
        MoveTowards(waypoint);
    }

    void MoveTowards(Transform waypoint) {
        // Get distances
        float xDistance = waypoint.position.x - transform.position.x;
        float yDistance = waypoint.position.y - transform.position.y;

        // Find direction
        Vector2 moveDirection;

        if (Mathf.Abs(xDistance) >= Mathf.Abs(yDistance)){
            moveDirection = Vector2.right * Mathf.Sign(xDistance);
        }
        else {
            moveDirection = Vector2.up * Mathf.Sign(yDistance);
        }

        // Move
        rigidBody.velocity = moveDirection * moveSpeed;

        // Detect things standing on top
        MoveThingsOnTop(moveDirection * moveSpeed);
    }

    void StopMoving() {
        rigidBody.velocity = Vector2.zero;
    }

    void MoveThingsOnTop(Vector2 movement) {
        // Get everything on top
        RaycastHit2D[] hits = new RaycastHit2D[5];
        collider2d.Cast(Vector2.up, hits, objectSnapDistance);

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform == null) break;
            
            hit.rigidbody.velocity += movement;
        }
    }
}
