using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    // === INTERFACE

    // Waypoints container that should hold the waypoints to follow
    public GameObject WayPointsContainer;

    // === STATE 

    // Current waypoint to follow
    Transform waypoint;

    // === REFS

    RhythmicExecuter rhythmicExecuter;

    private void Start() {
        rhythmicExecuter = FindObjectOfType<RhythmicExecuter>();

        EnsureNotNull.Objects(rhythmicExecuter);

        // Set default beat action to follow waypoint
        // rhythmicExecuter.defaultBeatAction = MoveToWaypoint;

        // Set default counterbeat action to halt movement
        // rhythmicExecuter.defaultCounterbeatAction = StopMovement;
    }
}
