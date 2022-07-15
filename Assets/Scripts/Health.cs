using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // === INTERFACE

    // Which layers will kill on trigger enter
    public LayerMask killLayers;

    public void OnTriggerEnter2D(Collider2D otherCollider) {
        // Check if this collider is in the kill list
        if (ColliderInKillLayer(otherCollider)) Die();
    }

    bool ColliderInKillLayer(Collider2D otherCollider) {
        return killLayers == (killLayers | 1 << otherCollider.gameObject.layer);
    }

    void Die() {
        print("ouch!");
    }
}
