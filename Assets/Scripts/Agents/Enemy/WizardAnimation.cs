using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimation : AnimationManager
{
    // === ANIMATIONS

    const string SWIM = "Air Swim";
    const string IMPULSE = "Float Impulse";

    // === STATE

    // How many seconds after counterbeat to start impulse animation
    float impulseStartTimeAfterCounterbeat;

    // === REFS

    RhythmicExecuter rhythmicExecuter;
    
    override protected void OnStart() {
        rhythmicExecuter = transform.parent.GetComponent<RhythmicExecuter>();

        EnsureNotNull.Objects(rhythmicExecuter);

        rhythmicExecuter.OnEveryCounterbeat.AddListener(SetSwim);

        rhythmicExecuter.OnEveryBeat.AddListener(SetImpulse);
    }

    private void OnDestroy() {
        rhythmicExecuter?.OnEveryCounterbeat?.RemoveListener(SetSwim);
        rhythmicExecuter?.OnEveryBeat?.RemoveListener(SetImpulse);
    }

    void SetSwim() {
        SetAnimationState(SWIM);
    }

    void SetImpulse() {
        SetAnimationState(IMPULSE);
    }
}
