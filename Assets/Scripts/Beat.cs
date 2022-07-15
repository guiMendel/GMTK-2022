using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Beat : MonoBehaviour
{
    // === INTERFACE
    
    public float beatsPerSecond = 0.5f;

    public UnityEvent beatTrigger;

    // === REFS

    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(BeatTimer());
    }

    // Coroutine that triggers the beat method in it's beat time
    private IEnumerator BeatTimer() {
        // Get how many seconds should pass between beats
        float secondsPerBeat = 1 / beatsPerSecond;
        
        while (true) {
            float beatStart = DateTime.Now.Second;
            
            // Play kick
            audioSource.Play();
            
            // Debug.Log("BeatTimer");
            beatTrigger.Invoke();

            // How much time has already passed in this beat
            float elapsedTime = DateTime.Now.Second - beatStart;

            yield return new WaitForSeconds(secondsPerBeat - elapsedTime);
        }
    }
}
