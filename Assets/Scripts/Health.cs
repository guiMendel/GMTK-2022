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
    public UnityEvent onDeath;

    public void Start() {
        onDeath ??= new UnityEvent();
    }

    public void OnTriggerEnter2D(Collider2D otherCollider) {
        // Check if this collider is in the kill list
        if (ColliderInKillLayer(otherCollider)) onDeath.Invoke();;
    }

    bool ColliderInKillLayer(Collider2D otherCollider) {
        return killLayers == (killLayers | 1 << otherCollider.gameObject.layer);
    }
}
