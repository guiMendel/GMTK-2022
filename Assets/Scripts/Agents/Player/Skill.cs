using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill : MonoBehaviour
{
    public Sprite icon;
    
    // === REFS

    TheDie theDie;
    UniverseMapper universeMapper;
    protected RhythmicExecuter rhythmicExecuter;
    protected Movement movement;


    // === PROPERTIES

    public int DieValue {
        get {
            string name = this.GetType().ToString();
            
            if (universeMapper.InverseSkill.ContainsKey(name)) return  universeMapper.InverseSkill[name];
            else return 0;
        }
    }
    public bool IsActive => universeMapper.CurrentSkill == this;


    // === OVERRIDABLES

    protected abstract void OnStart();
    protected virtual void BeatAction() {}
    protected virtual void CounterbeatAction() {}
    protected virtual void OnActivate() {}
    protected virtual void OnDeactivate() {}


    private void Start() {
        universeMapper = FindObjectOfType<UniverseMapper>();
        theDie = FindObjectOfType<TheDie>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();

        EnsureNotNull.Objects(universeMapper, theDie, rhythmicExecuter, movement);

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
        if (newValue == DieValue) OnActivate();

        else if (oldValue == DieValue) OnDeactivate();        
    }
}
