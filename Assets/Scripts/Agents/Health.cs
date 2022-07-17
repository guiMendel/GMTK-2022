using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // === INTERFACE

    // Which layers will kill on trigger enter
    public LayerMask killLayers;

    // Execute instantly on death
    public UnityEvent OnDeath;


    // === STATE

    public bool isDead;
    

    public void Start() {
        OnDeath ??= new UnityEvent();
    }

    public void OnTriggerEnter2D(Collider2D otherCollider) {
        // Check if this collider is in the kill list
        if (ColliderInKillLayer(otherCollider)) {
            OnDeath.Invoke();

            isDead = true;

            // Warn collided thing
            otherCollider.gameObject.SendMessage("OnHitPlayer", SendMessageOptions.DontRequireReceiver);
        }
    }

    bool ColliderInKillLayer(Collider2D otherCollider) {
        return killLayers == (killLayers | 1 << otherCollider.gameObject.layer);
    }
}
