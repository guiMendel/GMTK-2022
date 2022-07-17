using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : Skill
{
    // === INTERFACE

    public Animator shieldAnimator;


    // === ANIMATIONS

    const string ACTIVE = "Player Shield Active";
    const string FADE = "Player Shield Fade";
    
    
    // === REFS

    Health health;
    
    
    protected override void OnStart() {
        health = GetComponent<Health>();

        EnsureNotNull.Objects(health);
    }

    protected override void OnActivate() {
        health.Invulnerable = true;

        shieldAnimator.Play(ACTIVE);
    }

    protected override void OnDeactivate() {
        health.Invulnerable = false;
        
        shieldAnimator.Play(FADE);
    }
}
