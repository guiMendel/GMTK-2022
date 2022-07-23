using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random=UnityEngine.Random;

public class Beat : MonoBehaviour
{
    // === INTERFACE

    // BPM of the song
    [Min(1f)] public float beatsPerMinute = 120f;

    public UnityEvent beatTrigger;
    public UnityEvent counterbeatTrigger;

    public GameObject kickObject;
    public GameObject hatObject;
    public GameObject songObject;

    // === STATE

    // Position of the song, in seconds
    float songPosition;

    // Song position, in beats
    float songPositionBeats;

    // Seconds elapsed since song start
    float dspTime;
    
    // Duration of a beat
    float SecondsPerBeat => 60f / beatsPerMinute;

    // Duration of a move cycle, in seconds
    public float SecondsPerCycle => SecondsPerBeat * 8f;

    // === REFS

    AudioSource kickSource;
    AudioSource hatSource;
    AudioSource songSource;
    TheDie theDie;

    // Start is called before the first frame update
    void Awake()
    {
        theDie = FindObjectOfType<TheDie>();
        kickSource = kickObject.GetComponent<AudioSource>();
        hatSource = hatObject.GetComponent<AudioSource>();
        songSource = songObject.GetComponent<AudioSource>();
        
        beatTrigger ??= new UnityEvent();
        counterbeatTrigger ??= new UnityEvent();

        EnsureNotNull.Objects(kickSource, hatSource, theDie);
    }

    private void Start() {

        // Get song start time
        dspTime = (float) AudioSettings.dspTime;

        // Start song
        songSource.Play();
        
        StartCoroutine(BeatTimer());
    }

    // Coroutine that triggers the beat method in it's beat time
    private IEnumerator BeatTimer() {
        // Whether is triggering beat or counterbeat
        bool isCounter = false;

        // Calculate loop time (every 4 beats)
        float loopTime = SecondsPerBeat * 4f;
        
        while (true) {
            float beatStart = DateTime.Now.Second;

            // If counterbeat, first of all we let the die know
            if (isCounter) theDie.CheckRollDie();
            
            // Play sfx
            if (isCounter) hatSource.Play();
            else kickSource.Play();
            
            // Raise corresponding event
            if (isCounter) counterbeatTrigger.Invoke();
            else beatTrigger.Invoke();

            // Switch mode
            isCounter = !isCounter;

            // How much time has already passed in this cycle
            float elapsedTime = DateTime.Now.Second - beatStart;

            yield return new WaitForSeconds(loopTime - elapsedTime);
        }
    }
}
