using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmicExecuter : MonoBehaviour
{
    // === INTERFACE

    public UnityAction defaultBeatAction = () => {};
    public UnityAction defaultCounterbeatAction = () => {};
    
    // === STATE

    // Action to execute on next beat
    Dictionary<string, UnityAction> beatActions;

    // Action to execute on next counterbeat
    Dictionary<string, UnityAction> counterbeatActions;

    // === REFS

    Grid grid;
    Beat beat;

    public void Start() {
        beatActions = new Dictionary<string, UnityAction>();
        counterbeatActions = new Dictionary<string, UnityAction>();
        
        grid = FindObjectOfType<Grid>();
        beat = FindObjectOfType<Beat>();

        // Keep bodies snapped
        SnapToGrid();

        // Subscribe to beats
        beat.beatTrigger.AddListener(BeatListener);
        beat.counterbeatTrigger.AddListener(CounterbeatListener);
    }

    public void OnDestroy() {
        // Clean listeners up
        beat.beatTrigger.RemoveListener(BeatListener);
        beat.counterbeatTrigger.RemoveListener(CounterbeatListener);
    }

    public void AddBeatAction(string key, UnityAction newAction) {
        beatActions[key] = newAction;
    }


    public void AddCounterbeatAction(string key, UnityAction newAction) {
        counterbeatActions[key] = newAction;
    }

    public void ClearBeatActions() {
        beatActions.Clear();
    }

    public void ClearCounterbeatActions() {
        counterbeatActions.Clear();
    }

    public void ClearAll() {
        ClearBeatActions();
        ClearCounterbeatActions();
    }

    void BeatListener() {
        SnapToGrid();

        if (beatActions.Count == 0) {
            if (defaultBeatAction != null) defaultBeatAction();
        } 
        
        else PerformActions(beatActions);
    }

    void CounterbeatListener() {
        SnapToGrid();


        if (counterbeatActions.Count == 0) {
            if (defaultCounterbeatAction != null) defaultCounterbeatAction();
        } 
        
        else PerformActions(counterbeatActions);
    }

    // Performs the current beat action
    void PerformActions(Dictionary<string, UnityAction> actions) {
        foreach(var listener in actions.Values) listener();

        // Clear list
        actions.Clear();
    }

    // Snaps to grid position
    void SnapToGrid() {
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }
}
