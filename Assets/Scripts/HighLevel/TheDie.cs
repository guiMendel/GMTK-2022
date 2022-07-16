using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TheDie : MonoBehaviour
{
    // === INTERFACE

    [Range(1, 3)] public int beatsPerRoll = 1;

    [Range(1, 6)] public int totalFaces = 4;

    public Event.Int OnDieRoll;
    
    // === STATE

    // The current die value
    public int Value { get; private set; }

    // How many beats until next roll
    int beatsUntilNextRoll;
    
    // === REFS

    RhythmicExecuter rhythmicExecuter;

    
    // Start is called before the first frame update
    void Start()
    {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();

        OnDieRoll ??= new Event.Int();

        EnsureNotNull.Objects(rhythmicExecuter); 

        // Get die roll result every counterbeat
        rhythmicExecuter.OnIdleCounterbeat.AddListener(CheckRollDie);

        // Initialize die value
        beatsUntilNextRoll = beatsPerRoll;
        
        CheckRollDie();
    }

    void CheckRollDie() {
        // Count this beat to the beats left until next roll
        if (--beatsUntilNextRoll > 0) return;
        
        // Get a random number between 1 and 6
        Value = Random.Range(1, totalFaces + 1);

        OnDieRoll.Invoke(Value);

        beatsUntilNextRoll = beatsPerRoll;
    }
}
