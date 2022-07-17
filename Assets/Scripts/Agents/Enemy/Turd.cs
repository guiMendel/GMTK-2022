using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turd : MonoBehaviour
{
    // === INTERFACE

    public int beatsToLive = 3;
    
    
    // === REFS

    RhythmicExecuter rhythmicExecuter;
    Collider2D collider2d;
    ParticleSystem turdExplosion;
    SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        collider2d = GetComponent<Collider2D>();
        turdExplosion = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        EnsureNotNull.Objects(rhythmicExecuter, collider2d, turdExplosion, spriteRenderer);

        rhythmicExecuter.OnEveryBeat.AddListener(CountBeat);
    }

    private void OnDestroy() {
        rhythmicExecuter?.OnEveryBeat?.RemoveListener(CountBeat);
    }

    void CountBeat() {
        if (beatsToLive-- != 0) return;

        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct() {
        // Disable collision
        collider2d.enabled = false;

        // Disable sprite
        spriteRenderer.enabled = false;

        // Play destruction vfx
        turdExplosion.Play();

        // Wait
        yield return new WaitForSeconds(2f);

        // Destroy
        Destroy(gameObject);
    }

    public void OnHitPlayer() {
        beatsToLive = -1;
        StartCoroutine(SelfDestruct());
    }
}
