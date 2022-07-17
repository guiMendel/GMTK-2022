using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyYeti : EnemyType
{
    // === INTERFACE

    public GameObject turd;

    
    // === REFS

    Grid grid;


    // === PROPERTIES

    Vector3 centeredPosition => grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
    
    public override UniverseType HomeUniverse => FindObjectOfType<UniverseBlizzard>();

    protected override void OnStart() {
        grid = FindObjectOfType<Grid>();

        EnsureNotNull.Objects(grid);
    }

    protected override void BeatAction() {
        // Create a turd where is standing
        Instantiate(
            turd,
            centeredPosition + new Vector3(-movement.Direction * grid.cellSize.x, -0.22f, 0),
            Quaternion.identity
        );
    }
}
