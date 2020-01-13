using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        pauseCanvas.transform.parent = transform;
        pauseCanvas.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if (paused) {
                pauseCanvas.enabled = true;
                timeScaler.SetTimeScale(0);
            } else {
                pauseCanvas.enabled = false;
                timeScaler.SetTimeScale(1);
            }
        }
    }
}
