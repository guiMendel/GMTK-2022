using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UniverseDieMap : MonoBehaviour
{
    // === STATE

    // Maps a die face to a Universe Type
    Dictionary<int, UniverseType> typeMap;

    // Holds all universe types
    UniverseType[] universeTypes;


    // === REFS

    TheDie theDie;


    // === PROPERTIES

    public UniverseType CurrentUniverse => typeMap[theDie.Value];


    private void Start() {
        typeMap = new Dictionary<int, UniverseType>();
        
        theDie = FindObjectOfType<TheDie>();
        universeTypes = GetComponents<UniverseType>();

        EnsureNotNull.Objects(theDie);

        // Initialize mapping
        Shuffle();
    }

    void Shuffle() {
        // Get shuffled universe types
        UniverseType[] shuffledTypes = universeTypes.OrderBy((x) => Random.Range(0, 100)).ToArray();

        for (int i = 0; i < theDie.totalFaces; i++) {
            typeMap[i + 1] = shuffledTypes[i];
            // print(shuffledTypes[i] + ": " + (i+1));
        }
    }
}
