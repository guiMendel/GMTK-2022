using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimation : AnimationManager
{
    // === ANIMATIONS

    const string SWIM = "Air Swim";
    const string IMPULSE = "Float Impulse";
    const string SPAWN = "Wizard Spawn";

    // === STATE

    // How many seconds after counterbeat to start impulse animation
    float impulseStartTimeAfterCounterbeat;

    // === REFS

    RhythmicExecuter rhythmicExecuter;
    Wizard wizard;
    
    override protected void OnStart() {
        wizard = transform.parent.GetComponent<Wizard>();
        rhythmicExecuter = transform.parent.GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(rhythmicExecuter);

        rhythmicExecuter.OnEveryCounterbeat.AddListener(SetSwim);

        rhythmicExecuter.OnEveryBeat.AddListener(SetImpulse);
    }

    public void StartMoving() {
        wizard.BeginAction();
    }

    private void OnDestroy() {
        rhythmicExecuter?.OnEveryCounterbeat?.RemoveListener(SetSwim);
        rhythmicExecuter?.OnEveryBeat?.RemoveListener(SetImpulse);
    }

    void SetSwim() {
        if (wizard.active)        
        SetAnimationState(SWIM);
    }

    void SetImpulse() {
        if (wizard.active)        
        SetAnimationState(IMPULSE);
    }
}
