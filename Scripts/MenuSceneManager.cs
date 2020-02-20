using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {
    public void QuitToMainMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("_Start");
    }

    public void ForceRestartLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
