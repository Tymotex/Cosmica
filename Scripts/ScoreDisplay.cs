using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    LevelStatus levelStatus;
    [SerializeField] Text scoreText = null;

    void Start() {
        levelStatus = FindObjectOfType<LevelStatus>();
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        scoreText.text = levelStatus.score.ToString();
    }
}
