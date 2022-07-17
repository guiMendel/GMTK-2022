using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyType : MonoBehaviour
{
    // === INTERFACE

    public SpriteRenderer spriteRenderer;

    public ParticleSystem enterVFX;
    
    
    // === REFS

    UniverseDieMap universeDieMap;
    protected RhythmicExecuter rhythmicExecuter;
    protected Movement movement;


    // === PROPERTIES

    public bool IsActive => universeDieMap.CurrentUniverse == HomeUniverse;
    

    // === OVERRIDABLES
    
    public abstract UniverseType HomeUniverse { get; }

    abstract protected void BeatAction();

    protected abstract void OnStart();

    
    private void Start() {
        universeDieMap = FindObjectOfType<UniverseDieMap>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();

        EnsureNotNull.Objects(universeDieMap, rhythmicExecuter, movement);

        // Every counterbeat, prepare for this beat's action
        rhythmicExecuter.OnEveryCounterbeat.AddListener(ActionPrepare);

        OnStart();
    }

    private void OnDestroy() {
        // Clean up
        rhythmicExecuter?.OnEveryCounterbeat?.RemoveListener(ActionPrepare);
    }

    void ActionPrepare() {
        // If not active, do nothing
        if (IsActive == false) {
            spriteRenderer.enabled = false;
            return;
        }

        if (spriteRenderer.enabled == false)  {
            enterVFX.Play();
        }
        
        spriteRenderer.enabled = true;

        // Play anticipation animation
        // print(FindObjectOfType<TheDie>().Value + ": " + this.GetType().Name);

        // Set up a beat action
        rhythmicExecuter.AddBeatAction("enemyAction", BeatAction);
    }
}
