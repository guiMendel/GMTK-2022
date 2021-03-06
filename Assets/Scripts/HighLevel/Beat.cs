using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random=UnityEngine.Random;

public class Beat : MonoBehaviour
{
    // === INTERFACE
    
    public float secondsPerBeat = 1f;

    public UnityEvent beatTrigger;
    public UnityEvent counterbeatTrigger;

    public GameObject kickObject;
    public GameObject hatObject;

    // === REFS

    AudioSource kickSource;
    AudioSource hatSource;
    TheDie theDie;

    // Start is called before the first frame update
    void Start()
    {
        theDie = FindObjectOfType<TheDie>();
        kickSource = kickObject.GetComponent<AudioSource>();
        hatSource = hatObject.GetComponent<AudioSource>();
        beatTrigger ??= new UnityEvent();
        counterbeatTrigger ??= new UnityEvent();

        EnsureNotNull.Objects(kickSource, hatSource, theDie);

        StartCoroutine(BeatTimer());
    }

    // Coroutine that triggers the beat method in it's beat time
    private IEnumerator BeatTimer() {
        // Whether is triggering beat or counterbeat
        bool isCounter = false;

        // Calculate loop time (is half beat so that we can raise counterbeat events)
        float loopTime = secondsPerBeat / 2.0f;
        
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
