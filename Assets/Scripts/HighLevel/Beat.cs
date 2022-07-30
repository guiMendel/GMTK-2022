using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Beat : MonoBehaviour
{
  // === INTERFACE

  // BPM of the song
  [Min(1f)] public float beatsPerMinute = 120f;

  // Reference of BPM at which the timescale should be 1
  public float referenceBpm = 120f;

  // How many beats in a downbeat cycle
  public float beatsPerCycle = 8f;

  // Delay before start counting the beats, measured in beats
  public float beatCountStartDelay = 0.5f;

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
  float SongPosition => (float)AudioSettings.dspTime - initialDspTime;


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

    SetTimescale();
  }

  private void Start()
  {
    // Get song start time
    initialDspTime = (float)AudioSettings.dspTime;

    // Start song
    songSource.Play();

    // Init fields
    nextDownbeat = beatCountStartDelay;
    nextUpbeat = beatsPerCycle / 2f + beatCountStartDelay;
  }

  private void Update()
  {
    // Check if reached next downbeat time
    if (SongPositionBeats >= nextDownbeat)
    {
      // Advance downbeat
      while (SongPositionBeats >= nextDownbeat)
      {
        nextDownbeat += beatsPerCycle;
      }

      Downbeat();
    }

    // Check if reached next upbeat time
    if (SongPositionBeats >= nextUpbeat)
    {
      // Advance upbeat
      while (SongPositionBeats >= nextUpbeat)
      {
        nextUpbeat += beatsPerCycle;
      }

      Upbeat();
    }
  }

  void Downbeat()
  {
    // First, let the die know
    theDie.CheckRollDie();

    // kickSource.Play();
    // print("DownbeatTrigger");
    DownbeatTrigger.Invoke();
  }

  void Upbeat()
  {
    // hatSource.Play();
    // print("UpbeatTrigger");
    UpbeatTrigger.Invoke();
  }

  // Sets the timescale relative to the song's bpm
  void SetTimescale()
  {
    Time.timeScale = beatsPerMinute / referenceBpm;
  }
}
