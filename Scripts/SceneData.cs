using UnityEngine;
using System.Collections;

// Manually set the fields in the inspector. The fields tell the "success" and "failure" scenes
// what to display when they are transitioned to
public class SceneData : MonoBehaviour {
    public string currSceneZone;   // Manually set in the inspector
    public string currSceneLevel;  // Manually set in the inspector
    public int playerScore;        // Fetched from LevelStatus
    public int timeTaken;

    void Start() {
        SceneDataManager manager = FindObjectOfType<SceneDataManager>();
        manager.currSceneZone = currSceneZone;
        manager.currSceneLevel = currSceneLevel;
    }

    public void SetScore(int score) {
        playerScore = score;
    }

    public void SetTimeTaken(int timeElapsed) {
        timeTaken = timeElapsed;
    }
}