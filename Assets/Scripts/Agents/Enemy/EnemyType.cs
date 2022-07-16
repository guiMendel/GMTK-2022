using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyType : MonoBehaviour
{
    // === REFS

    Beat beat;
    UniverseDieMap universeDieMap;
    RhythmicExecuter rhythmicExecuter;


    // === PROPERTIES

    public bool IsActive => universeDieMap.CurrentUniverse == HomeUniverse;
    

    // === OVERRIDABLES
    
    public abstract UniverseType HomeUniverse { get; }

    abstract protected void BeatAction();

    
    private void Start() {
        beat = FindObjectOfType<Beat>();
        universeDieMap = FindObjectOfType<UniverseDieMap>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(beat, universeDieMap, rhythmicExecuter);

        // Every counterbeat, prepare for this beat's action
        beat.counterbeatTrigger.AddListener(ActionPrepare);
    }

    private void OnDestroy() {
        // Clean up
        beat.counterbeatTrigger.RemoveListener(ActionPrepare);
    }

    void ActionPrepare() {
        // If not active, do nothing
        if (IsActive == false) return;

        // Play anticipation animation
        // print(FindObjectOfType<TheDie>().Value + ": " + this.GetType().Name);

        // Set up a beat action
        rhythmicExecuter.AddBeatAction("enemyAction", BeatAction);
    }
}
