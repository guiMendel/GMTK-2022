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

    RhythmicExecuter rhythmicExecuter;
    Movement movement;
    Grid grid;

    private void Start() {
        rhythmicExecuter = FindObjectOfType<RhythmicExecuter>();
        movement = GetComponent<Movement>();
        grid = FindObjectOfType<Grid>();

        EnsureNotNull.Objects(rhythmicExecuter, movement, grid);

        // Set default beat action to follow waypoint
        rhythmicExecuter.OnIdleBeat.AddListener(MoveToWaypoint);
    }

    private void OnDestroy() {
        // Clean up listeners
        rhythmicExecuter?.OnIdleBeat?.RemoveListener(MoveToWaypoint);
    }

    void MoveToWaypoint() {
        // Get waypoint
        Transform waypoint = WaypointsContainer.GetChild(waypointIndex);

        // Detect arrival
        print((grid.WorldToCell(transform.position), grid.WorldToCell(waypoint.position)));
        
        if (grid.WorldToCell(transform.position) == grid.WorldToCell(waypoint.position)) {
            // Advance waypoint
            waypointIndex = (waypointIndex + 1) % WaypointsContainer.childCount;

            waypoint = WaypointsContainer.GetChild(waypointIndex);
        }

        // Get waypoint direction
        float direction = Mathf.Sign(waypoint.position.x - transform.position.x);

        // Move towards it
        movement.MakeMove(direction)();
    }
}
