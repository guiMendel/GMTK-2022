using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManualAnimation : MonoBehaviour
{
    // === INTERFACE
    
    public Sprite counterbeatFrame;
    public Sprite beatFrame;


    // === REFS

    RhythmicExecuter rhythmicExecuter;
    SpriteRenderer spriteRenderer;

    private void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        EnsureNotNull.Objects(rhythmicExecuter, spriteRenderer);

        rhythmicExecuter.OnEveryBeat.AddListener(SetBeatFrame);
        rhythmicExecuter.OnEveryCounterbeat.AddListener(SetCounterbeatFrame);
    }

    private void OnDestroy() {
        rhythmicExecuter.OnEveryBeat.RemoveListener(SetBeatFrame);
        rhythmicExecuter.OnEveryCounterbeat.RemoveListener(SetCounterbeatFrame);
    }

    void SetBeatFrame() {
        spriteRenderer.sprite = beatFrame;
    }

    void SetCounterbeatFrame() {
        spriteRenderer.sprite = counterbeatFrame;
    }
}
