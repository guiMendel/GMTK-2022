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

    // How many seconds after downbeat to start impulse animation
    float impulseStartTimeAfterDownbeat;

    // === REFS

    RhythmicExecuter rhythmicExecuter;
    Wizard wizard;
    
    override protected void OnStart() {
        wizard = transform.parent.GetComponent<Wizard>();
        rhythmicExecuter = transform.parent.GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(rhythmicExecuter);

        rhythmicExecuter.OnEveryDownbeat.AddListener(SetSwim);

        rhythmicExecuter.OnEveryUpbeat.AddListener(SetImpulse);

        wizard.OnReset.AddListener(SetSpawn);
    }

    public void StartMoving() {
        wizard.BeginAction();
    }

    private void OnDestroy() {
        rhythmicExecuter?.OnEveryDownbeat?.RemoveListener(SetSwim);
        rhythmicExecuter?.OnEveryUpbeat?.RemoveListener(SetImpulse);
        wizard?.OnReset?.RemoveListener(SetSpawn);
    }

    void SetSwim() {
        if (wizard.active)        
        SetAnimationState(SWIM);
    }

    void SetImpulse() {
        if (wizard.active)        
        SetAnimationState(IMPULSE);
    }

    void SetSpawn() {
        SetAnimationState(SPAWN);
    }
}
