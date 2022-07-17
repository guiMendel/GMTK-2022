using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UniverseMapper : MonoBehaviour
{
    // === INTERFACE

    public Image[] skillSlots;
    
    
    // === STATE

    // Maps a die face to a Universe Type
    public Dictionary<int, UniverseType> Type { get; private set; }
    public Dictionary<string, int> InverseType { get; private set; }

    // Maps a die face to a Player Skill
    public Dictionary<int, Skill> SkillMap { get; private set; }
    public Dictionary<string, int> InverseSkill { get; private set; }

    // Holds all universe types
    UniverseType[] universeTypes;
    
    // Holds all skills
    Skill[] skills;


    // === REFS

    TheDie theDie;


    // === PROPERTIES

    public UniverseType CurrentUniverse {
        get {
            if (theDie.Value == 0 || !Type.ContainsKey(theDie.Value)) return null;
            
            return Type[theDie.Value];
        }
    }

    public Skill CurrentSkill {
        get {
            if (theDie.Value == 0 || !SkillMap.ContainsKey(theDie.Value)) return null;
            
            return SkillMap[theDie.Value];
        }
    }


    private void Awake() {
        Type = new Dictionary<int, UniverseType>();
        InverseType = new Dictionary<string, int>();
        SkillMap = new Dictionary<int, Skill>();
        InverseSkill = new Dictionary<string, int>();
        
        theDie = FindObjectOfType<TheDie>();
        universeTypes = GetComponents<UniverseType>();
        skills = FindObjectOfType<PlayerController>().transform.GetComponents<Skill>();

        EnsureNotNull.Objects(theDie);
    }

    private void Start() {
        // Initialize mapping
        Shuffle(Type, InverseType, universeTypes);
        Shuffle(SkillMap, InverseSkill, skills);
        ShowUISkills();
    }

    void Shuffle<T>(
        Dictionary<int, T> map, Dictionary<string, int> inverseMap, T[] options
    ) {
        // Get shuffled universe types
        T[] shuffledOptions = options.OrderBy((x) => Random.Range(0, 100)).ToArray();

        for (int i = 0; i < theDie.totalFaces; i++) {
            string optionName = shuffledOptions[i].GetType().ToString();
            
            map[i + 1] = shuffledOptions[i];
            inverseMap[optionName] = i + 1;
            
            // print(optionName + ": " + (i+1));
        }
    }

    void ShowUISkills() {
        for (int i = 0; i < skillSlots.Length; i++) {
            skillSlots[i].sprite = SkillMap[i+1].icon;
        }
    }
}
