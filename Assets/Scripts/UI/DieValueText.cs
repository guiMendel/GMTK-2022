using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DieValueText : MonoBehaviour
{
    // === INTERFACE

    public TMP_Text textComponent;
    
    // === REFS

    TheDie theDie;
    
    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        theDie = FindObjectOfType<TheDie>();

        EnsureNotNull.Objects(textComponent, theDie);

        theDie.OnDieRoll.AddListener(SynchronizeText);

        SynchronizeText(theDie.Value);
    }

    private void OnDestroy() {
        theDie?.OnDieRoll?.RemoveListener(SynchronizeText);
    }

    public void SynchronizeText(int value, int oldValue = 0) {
        textComponent.text = value.ToString();
    }
}
