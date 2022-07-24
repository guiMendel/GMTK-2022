using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyType : MonoBehaviour
{
    // === INTERFACE

    public SpriteRenderer spriteRenderer;

    public ParticleSystem enterVFX;
    
    
    // === REFS

    EnemyFreezer enemyFreezer;
    UniverseMapper UniverseMapper;
    protected RhythmicExecuter rhythmicExecuter;
    protected Movement movement;


    // === PROPERTIES

    public bool IsActive => UniverseMapper.CurrentUniverse == HomeUniverse;
    

    // === OVERRIDABLES
    
    public abstract UniverseType HomeUniverse { get; }

    abstract protected void BeatAction();

    protected abstract void OnStart();


    void BeatActionWrapper() {
        if (enemyFreezer.frozen) {
            enemyFreezer.Unfreeze();
            
            return;
        }

        BeatAction();
    }

    
    private void Start() {
        UniverseMapper = FindObjectOfType<UniverseMapper>();
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        movement = GetComponent<Movement>();
        enemyFreezer = GetComponentInChildren<EnemyFreezer>();

        EnsureNotNull.Objects(UniverseMapper, rhythmicExecuter, movement, enemyFreezer);

        // Every downbeat, prepare for this beat's action
        rhythmicExecuter.OnEveryDownbeat.AddListener(ActionPrepare);

        OnStart();
    }

    private void OnDestroy() {
        // Clean up
        rhythmicExecuter?.OnEveryDownbeat?.RemoveListener(ActionPrepare);
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
        rhythmicExecuter.AddUpbeatAction("enemyAction", BeatActionWrapper);
    }
}
