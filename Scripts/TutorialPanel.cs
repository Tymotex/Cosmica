using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPanel : MonoBehaviour {
    public Vector3 panelOffset;

    public void LoadNextPanel() {
        transform.GetComponentInParent<Tutorial>().ProgressToNextTutorial();
    }

    public void EndTutorial() {
        SceneManager.LoadScene("_Start");
    }
}
