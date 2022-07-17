using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezer : MonoBehaviour
{
    // === STATE

    public bool frozen;

    SpriteRenderer[] spriteRenderers;

    private void Start() {
        spriteRenderers = transform.parent.GetComponentsInChildren<SpriteRenderer>();
    }


    public void Freeze() {
        foreach (var sprite in spriteRenderers) {
            sprite.color = Color.blue;
        }

        frozen = true;
    }

    public void Unfreeze() {
        foreach (var sprite in spriteRenderers) {
            sprite.color = Color.white;
        }

        frozen = false;
    }
}
