using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : Skill
{
    // === INTERFACE

    public Animator shieldAnimator;
    public AudioSource sfx;


    // === ANIMATIONS

    const string ACTIVE = "Shield";
    const string FADE = "Shield Vanish";
    
    
    // === REFS

    Health health;
    
    
    protected override void OnStart() {
        health = GetComponent<Health>();

        EnsureNotNull.Objects(health);
    }

    protected override void OnActivate() {
        health.Invulnerable = true;

        sfx.Play();

        shieldAnimator.Play(ACTIVE);
    }

    protected override void OnDeactivate() {
        health.Invulnerable = false;
        
        shieldAnimator.Play(FADE);
    }
}
