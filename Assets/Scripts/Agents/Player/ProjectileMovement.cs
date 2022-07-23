using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float direction = 1f;
    public int tilesPerBeat = 1;
    public GameObject explosionVFX;

    float moveSpeed;
    
    
    RhythmicExecuter rhythmicExecuter;
    Rigidbody2D rigidBody;

    private void Start() {
        rhythmicExecuter = GetComponent<RhythmicExecuter>();
        rigidBody = GetComponent<Rigidbody2D>();

        rhythmicExecuter.OnEveryBeat.AddListener(Move);


        Grid grid = FindObjectOfType<Grid>();
        Beat beat = FindObjectOfType<Beat>();

        moveSpeed = tilesPerBeat * grid.cellSize.x / (beat.SecondsPerCycle / 4.0f);
    }

    private void OnDestroy() {
        rhythmicExecuter.OnEveryBeat.RemoveListener(Move);
    }

    void Move() {
        rigidBody.velocity = Vector3.right * moveSpeed * direction;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyFreezer hitEnemy = other.GetComponent<EnemyFreezer>();
        
        if (hitEnemy != null) hitEnemy.Freeze();
        
        var explosion = Instantiate(explosionVFX, transform.position, transform.rotation);

        Destroy(explosion, 2f);
        
        Destroy(gameObject);
    }
}
