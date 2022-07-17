using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    // === INTERFACE

    // Waypoints container that should hold the waypoints to follow
    public Transform WaypointsContainer;

    // === STATE 

    // Current waypoint to follow
    int waypointIndex = 0;

    // === REFS

    Beat beat;
    Movement movement;
    Grid grid;

    private void Start() {
        beat = FindObjectOfType<Beat>();
        movement = GetComponent<Movement>();
        grid = FindObjectOfType<Grid>();

        EnsureNotNull.Objects(beat, movement, grid);

        // Set default beat action to follow waypoint
        beat.beatTrigger.AddListener(MoveToWaypoint);
    }

    private void OnDestroy() {
        // Clean up listeners
        beat?.beatTrigger?.RemoveListener(MoveToWaypoint);
    }

    void MoveToWaypoint() {
        print(12312);
        
        // Get waypoint
        Transform waypoint = WaypointsContainer.GetChild(waypointIndex);

        // Detect arrival
        if (grid.WorldToCell(transform.position) == grid.WorldToCell(waypoint.position)) {
            // Advance waypoint
            waypointIndex = (waypointIndex + 1) % WaypointsContainer.childCount;

            waypoint = WaypointsContainer.GetChild(waypointIndex);
        }

        // Get waypoint direction
        float direction = Mathf.Sign(waypoint.position.x - transform.position.x);

        movement.Direction = direction;

        // If obstructed, jump
        if (movement.IsObstructed()) {
            movement.MakeJump()();
            movement.MakeMove(direction, hop: false)();
        }

        // Move towards it
        else movement.MakeMove(direction)();
    }
}
