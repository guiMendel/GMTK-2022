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

    // Reference of BPM at which the timescale should be 1
    public float referenceBpm = 120f;

    // How many beats in a downbeat cycle
    public float beatsPerCycle = 8f;

    public UnityEvent DownbeatTrigger;
    public UnityEvent UpbeatTrigger;

    public GameObject kickObject;
    public GameObject hatObject;
    public GameObject songObject;

    // === STATE

    // Seconds elapsed since song start
    float initialDspTime;

    // When to trigger next downbeat
    float nextDownbeat;

    // When to trigger next upbeat
    float nextUpbeat;


    // === PROPERTIES
    
    // Duration of a downbeat
    float SecondsPerBeat => 60f / beatsPerMinute;

    // Duration of a move cycle, in seconds
    public float SecondsPerCycle => SecondsPerBeat * beatsPerCycle;

    // Song position, in downbeats
    float SongPositionBeats => SongPosition / SecondsPerBeat;

    // Position of the song, in seconds
    float SongPosition => (float) AudioSettings.dspTime - initialDspTime;


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
        
        DownbeatTrigger ??= new UnityEvent();
        UpbeatTrigger ??= new UnityEvent();

        EnsureNotNull.Objects(kickSource, hatSource, theDie);
    }

    private void Start() {
        // Get song start time
        initialDspTime = (float) AudioSettings.dspTime;

        // Start song
        songSource.Play();

        // Register first downbeat
        Downbeat();

        // Init fields
        nextDownbeat = beatsPerCycle;
        nextUpbeat = beatsPerCycle / 2f;

        SetTimescale();
    }

    private void Update() {
        // When either a downbeat or upbeat triggers, this is incremented
        int triggered;

        // Whenever both down and upbeat trigger, recheck if another trigger is necessary
        do {
            triggered = 0;
            
            // Check if reached next downbeat time
            if (SongPositionBeats >= nextDownbeat) {
                // Advance downbeat
                nextDownbeat += beatsPerCycle;

                Downbeat();

                triggered++;
            }

            // Check if reached next upbeat time
            if (SongPositionBeats >= nextUpbeat) {
                // Advance upbeat
                nextUpbeat += beatsPerCycle;

                Upbeat();

                triggered++;
            }
        } while (triggered == 2);
    }

    void Downbeat() {
        // First, let the die know
        theDie.CheckRollDie();

        kickSource.Play();
        DownbeatTrigger.Invoke();
    }

    void Upbeat() {
        hatSource.Play();
        UpbeatTrigger.Invoke();
    }

    // Sets the timescale relative to the song's bpm
    void SetTimescale() {
        Time.timeScale = beatsPerMinute / referenceBpm;
    }
}
