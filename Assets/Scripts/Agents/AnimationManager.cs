using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AnimationManager : MonoBehaviour
{
    // === STATE 

    protected Animator animator;
    protected string currentState;

    // === OVERRIDABLES

    abstract protected void OnStart();
    

    private void Start() {
        animator = GetComponent<Animator>();

        EnsureNotNull.Objects(animator);

        OnStart();
    }

    protected void SetAnimationState(string state) {
        // Disregard redundancy
        if (currentState == state) return;
        
        animator.Play(state);
        currentState = state;
    }

    public void AnnounceAnimationStateChange(string state) {
        currentState = state;
    }
}
