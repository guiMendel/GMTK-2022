using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UniverseType : MonoBehaviour
{
    // === INTERFACE
    
    public List<GameObject> synchronizeObjects;


    // === REFS

    UniverseDieMap universeDieMap;
    TheDie theDie;


    // === PROPERTIES

    public int DieValue => universeDieMap.InverseTypeMap[this.GetType().ToString()];
    public bool IsActive => universeDieMap.CurrentUniverse == this;


    // === OVERRIDABLES

    protected abstract void OnStart();


    private void Start() {
        universeDieMap = FindObjectOfType<UniverseDieMap>();
        theDie = FindObjectOfType<TheDie>();

        EnsureNotNull.Objects(universeDieMap, theDie);

        SetSynchronizedObjects(IsActive);

        theDie.OnDieRoll.AddListener(WatchDieRoll);

        OnStart();
    }

    private void OnDestroy() {
        theDie?.OnDieRoll?.RemoveListener(WatchDieRoll);
    }

    void WatchDieRoll(int newValue, int oldValue) {
        // Ignore redundancy
        if (newValue == oldValue) return;
        
        // Enable when activated
        if (newValue == DieValue) SetSynchronizedObjects(true);

        // Disable when deactivated
        else if (oldValue == DieValue) SetSynchronizedObjects(false);
    }

    void SetSynchronizedObjects(bool enabled) {
        foreach (var synchronizeObject in synchronizeObjects) {
            synchronizeObject.SetActive(enabled);
        }
    }
}
