using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmicExecuter : MonoBehaviour
{
    // === INTERFACE

    // Triggered when no beat action is available on beat
    public UnityEvent OnIdleBeat;

    // Triggered when no counterbeat action is available on counterbeat
    public UnityEvent OnIdleCounterbeat;
    
    // === STATE

    // Action to execute on next beat
    Dictionary<string, UnityAction> beatActions;

    // Action to execute on next counterbeat
    Dictionary<string, UnityAction> counterbeatActions;

    // === REFS

    Grid grid;
    Beat beat;

    public void Start() {
        OnIdleBeat ??= new UnityEvent();
        OnIdleCounterbeat ??= new UnityEvent();
        
        beatActions = new Dictionary<string, UnityAction>();
        counterbeatActions = new Dictionary<string, UnityAction>();
        
        grid = FindObjectOfType<Grid>();
        beat = FindObjectOfType<Beat>();

        EnsureNotNull.Objects(grid, beat);

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

    public UnityAction GetBeatAction(string key) {
        return beatActions.ContainsKey(key) ? beatActions[key] : null;
    }


    public UnityAction GetCounterbeatAction(string key) {
        return counterbeatActions.ContainsKey(key) ? counterbeatActions[key] : null;
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
        PerformActions(beatActions, OnIdleBeat);
    }

    void CounterbeatListener() {
        PerformActions(counterbeatActions, OnIdleCounterbeat);
    }

    // Performs the current beat action
    void PerformActions(Dictionary<string, UnityAction> actions, UnityEvent OnIdle) {
        // Firstly, snap
        SnapToGrid();
        
        // If no actions, invoke idle event
        if (actions.Count == 0) {
            OnIdle.Invoke();
            return;
        }
        
        foreach(var listener in actions.Values) listener();

        // Clear list
        actions.Clear();
    }

    // Snaps to grid position
    void SnapToGrid() {
        transform.position = grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    }
}
