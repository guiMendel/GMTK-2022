using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManualAnimation : MonoBehaviour
{
    // === INTERFACE
    
    public Sprite downbeatFrame;
    public Sprite upbeatFrame;


    // === REFS

    RhythmicExecuter rhythmicExecuter;
    SpriteRenderer spriteRenderer;

    private void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        EnsureNotNull.Objects(rhythmicExecuter, spriteRenderer);

        rhythmicExecuter.OnEveryUpbeat.AddListener(SetBeatFrame);
        rhythmicExecuter.OnEveryDownbeat.AddListener(SetDownbeatFrame);
    }

    private void OnDestroy() {
        rhythmicExecuter.OnEveryUpbeat.RemoveListener(SetBeatFrame);
        rhythmicExecuter.OnEveryDownbeat.RemoveListener(SetDownbeatFrame);
    }

    void SetBeatFrame() {
        spriteRenderer.sprite = upbeatFrame;
    }

    void SetDownbeatFrame() {
        spriteRenderer.sprite = downbeatFrame;
    }
}
