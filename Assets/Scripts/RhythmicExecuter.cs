using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmicExecuter : MonoBehaviour
{
    // === INTERFACE

    public bool disableSnap;

    public Vector3 snapOffset;

    // Triggered when no beat action is available on beat
    public UnityEvent OnIdleDownbeat;

    // Triggered when no upbeat action is available on upbeat
    public UnityEvent OnIdleUpbeat;

    public UnityEvent OnEveryDownbeat;

    public UnityEvent OnEveryUpbeat;
    
    // === STATE

    // Action to execute on next beat
    Dictionary<string, UnityAction> downbeatActions;

    // Action to execute on next upbeat
    Dictionary<string, UnityAction> upbeatActions;

    // === REFS

    Grid grid;
    Beat beat;

    public void Start() {
        OnIdleDownbeat ??= new UnityEvent();
        OnIdleUpbeat ??= new UnityEvent();
        OnEveryDownbeat ??= new UnityEvent();
        OnEveryUpbeat ??= new UnityEvent();
        
        downbeatActions = new Dictionary<string, UnityAction>();
        upbeatActions = new Dictionary<string, UnityAction>();
        
        grid = FindObjectOfType<Grid>();
        beat = FindObjectOfType<Beat>();

        EnsureNotNull.Objects(grid, beat);

        // Keep bodies snapped
        SnapToGrid();

        // Subscribe to beats
        beat.DownbeatTrigger.AddListener(DownbeatListener);
        beat.UpbeatTrigger.AddListener(UpbeatListener);
    }

    public void OnDestroy() {
        // Clean listeners up
        beat.DownbeatTrigger.RemoveListener(DownbeatListener);
        beat.UpbeatTrigger.RemoveListener(UpbeatListener);
    }

    public void AddDownbeatAction(string key, UnityAction newAction) {
        downbeatActions[key] = newAction;
    }


    public void AddUpbeatAction(string key, UnityAction newAction) {
        upbeatActions[key] = newAction;
    }

    public UnityAction GetDownbeatAction(string key) {
        return downbeatActions.ContainsKey(key) ? downbeatActions[key] : null;
    }


    public UnityAction GetUpbeatAction(string key) {
        return upbeatActions.ContainsKey(key) ? upbeatActions[key] : null;
    }

    public void ClearDownbeatActions() {
        downbeatActions.Clear();
    }

    public void ClearUpbeatActions() {
        upbeatActions.Clear();
    }

    public void ClearAll() {
        ClearDownbeatActions();
        ClearUpbeatActions();
    }

    void DownbeatListener() {
        PerformActions(downbeatActions, OnEveryDownbeat, OnIdleDownbeat);
    }

    void UpbeatListener() {
        print("upbeat!");
        
        PerformActions(upbeatActions, OnEveryUpbeat, OnIdleUpbeat);
    }

    // Performs the current beat action
    void PerformActions(Dictionary<string, UnityAction> actions, UnityEvent always, UnityEvent OnIdle) {
        // Firstly, snap
        SnapToGrid();

        // Snap always event
        always.Invoke();
        
        // If no actions, invoke idle event
        if (actions.Count == 0) {
            OnIdle.Invoke();
            return;
        }

        // If has move action, perform it first
        if (actions.ContainsKey("move")) {
            actions["move"]();
            actions.Remove("move");
        }
        
        foreach(var listener in actions.Values) listener();

        // Clear list
        actions.Clear();
    }

    // Snaps to grid position
    void SnapToGrid() {
        if (disableSnap) return;
        
        transform.position =
            grid.GetCellCenterWorld(grid.WorldToCell(transform.position)) + snapOffset;
    }
}
