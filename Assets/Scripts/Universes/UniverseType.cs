using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UniverseType : MonoBehaviour
{
    // === INTERFACE
    
    public List<GameObject> synchronizeObjects;


    // === REFS

    UniverseMapper UniverseMapper;
    TheDie theDie;
    RhythmicExecuter rhythmicExecuter;


    // === PROPERTIES

    public int DieValue => UniverseMapper.InverseType[this.GetType().ToString()];
    public bool IsActive => UniverseMapper.CurrentUniverse == this;


    // === OVERRIDABLES

    protected abstract void OnStart();
    protected virtual void BeatAction() {}
    protected virtual void CounterbeatAction() {}
    protected virtual void OnActivate() {}


    private void Start() {
        UniverseMapper = FindObjectOfType<UniverseMapper>();
        theDie = FindObjectOfType<TheDie>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(UniverseMapper, theDie, rhythmicExecuter);

        SetSynchronizedObjects(IsActive);

        theDie.OnDieRoll.AddListener(WatchDieRoll);

        rhythmicExecuter.OnEveryBeat.AddListener(() => {
            if (IsActive) BeatAction();
        });

        rhythmicExecuter.OnEveryCounterbeat.AddListener(() => {
            if (IsActive) CounterbeatAction();
        });

        OnStart();
    }

    private void OnDestroy() {
        theDie?.OnDieRoll?.RemoveListener(WatchDieRoll);
    }

    void WatchDieRoll(int newValue, int oldValue) {
        // Ignore redundancy
        if (newValue == oldValue) return;
        
        // Enable when activated
        if (newValue == DieValue) {
            OnActivate();
            SetSynchronizedObjects(true);
        }

        // Disable when deactivated
        else if (oldValue == DieValue) SetSynchronizedObjects(false);
    }

    void SetSynchronizedObjects(bool enabled) {
        foreach (var synchronizeObject in synchronizeObjects) {
            synchronizeObject.SetActive(enabled);
        }
    }
}
