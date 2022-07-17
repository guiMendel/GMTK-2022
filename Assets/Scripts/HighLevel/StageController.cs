using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public void ResetStage() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadChallengeRoom() {
        int newScene = Random.Range(1, 4);

        while (newScene == SceneManager.GetActiveScene().buildIndex) {
            newScene = Random.Range(1, 4);
        }
        
        SceneManager.LoadScene(newScene);
    }
}
