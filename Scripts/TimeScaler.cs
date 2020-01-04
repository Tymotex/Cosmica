using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour {
    [SerializeField]
    [Tooltip("Sets the time scale. Designed for testing purposes only.")]
    float timeScale = 1;
    void Start() {
        Time.timeScale = timeScale;
    }
    private void Update() {
        Start();
    }
}
