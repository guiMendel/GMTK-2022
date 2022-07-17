using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static GameObject instance;

    // void Awake() {
    //     if (instance != null) {
    //         gameObject.SetActive(false);
    //         Destroy(gameObject);

    //         return;
    //     }

    //     instance = gameObject;        
    //     DontDestroyOnLoad(gameObject);
    // }
}
