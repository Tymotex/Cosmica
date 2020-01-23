using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    // ===== Game Pause =====
    [SerializeField]
    Canvas pauseCanvasPrefab = null;
    bool paused = false;
    [SerializeField]
    TimeScaler timeScaler = null;

    Canvas pauseCanvas = null;

    void Start() {
        pauseCanvas = Instantiate(pauseCanvasPrefab, Vector3.zero, Quaternion.identity) as Canvas;
        pauseCanvas.transform.SetParent(transform);
        pauseCanvas.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }
    }

    public void ToggleMenu() {
        paused = !paused;
        if (paused) {
            pauseCanvas.enabled = true;
            timeScaler.SetTimeScale(0f);
        } else {
            pauseCanvas.enabled = false;
            timeScaler.SetTimeScale(1);
        }
    }
}
