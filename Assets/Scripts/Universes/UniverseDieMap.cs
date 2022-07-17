using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UniverseDieMap : MonoBehaviour
{
    // === STATE

    // Maps a die face to a Universe Type
    public Dictionary<int, UniverseType> TypeMap { get; private set; }
    public Dictionary<string, int> InverseTypeMap { get; private set; }

    // Holds all universe types
    UniverseType[] universeTypes;


    // === REFS

    TheDie theDie;


    // === PROPERTIES

    public UniverseType CurrentUniverse => theDie.Value == 0 ? null : TypeMap[theDie.Value];


    private void Awake() {
        TypeMap = new Dictionary<int, UniverseType>();
        InverseTypeMap = new Dictionary<string, int>();
        
        theDie = FindObjectOfType<TheDie>();
        universeTypes = GetComponents<UniverseType>();

        EnsureNotNull.Objects(theDie);
    }

    private void Start() {
        // Initialize mapping
        Shuffle();
    }

    void Shuffle() {
        // Get shuffled universe types
        UniverseType[] shuffledTypes = universeTypes.OrderBy((x) => Random.Range(0, 100)).ToArray();

        for (int i = 0; i < theDie.totalFaces; i++) {
            string universeType = shuffledTypes[i].GetType().ToString();
            
            TypeMap[i + 1] = shuffledTypes[i];
            InverseTypeMap[universeType] = i + 1;
            
            print(universeType + ": " + (i+1));
        }
    }
}
