using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour {
    [SerializeField]
    [Tooltip("Sets the time scale. Designed for testing purposes only.")]
    float timeScale = 1;

    private void Update() {  // TODO: Change this to Start(). Shouldn't be calling this each frame
        // SetTimeScale(timeScale);
    }

    public void SetTimeScale(float newTimeScale) {
        Debug.Log("Setting new timescale: " + newTimeScale);

        Time.timeScale = newTimeScale;
    }
}
