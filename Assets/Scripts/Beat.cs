using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    public float beatsPerSecond = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeatTimer());
    }

    // Coroutine that triggers the beat method in it's beat time
    private IEnumerator BeatTimer() {
        // Get how many seconds should pass between beats
        float secondsPerBeat = 1 / beatsPerSecond;
        
        while (true) {
            float beatStart = DateTime.Now.Second;
            
            // Debug.Log("BeatTimer");

            // How much time has already passed in this beat
            float elapsedTime = DateTime.Now.Second - beatStart;

            yield return new WaitForSeconds(secondsPerBeat - elapsedTime);
        }
    }
}
