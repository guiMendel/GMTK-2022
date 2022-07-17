using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TheDie : MonoBehaviour
{
    // === INTERFACE

    [Range(1, 3)] public int beatsPerRoll = 1;

    [Range(1, 6)] public int totalFaces = 4;

    public Event.DoubleInt OnDieRoll;
    
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

        OnDieRoll ??= new Event.DoubleInt();

        EnsureNotNull.Objects(rhythmicExecuter); 

        // Initialize die value
        beatsUntilNextRoll = beatsPerRoll;
        
        CheckRollDie(dontCount: true);
    }

    public void CheckRollDie(bool dontCount = false) {
        // Count this beat to the beats left until next roll
        if (dontCount == false && --beatsUntilNextRoll > 0) return;
        
        // Get a random number between 1 and 6
        int oldValue = Value;
        Value = Random.Range(1, totalFaces + 1);

        OnDieRoll.Invoke(Value, oldValue);

        beatsUntilNextRoll = beatsPerRoll;
    }
}
