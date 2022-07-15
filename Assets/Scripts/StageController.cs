using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    public void ResetStage() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
