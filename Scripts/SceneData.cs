using UnityEngine;
using System.Collections;

// Manually set the fields in the inspector. The fields tell the "success" and "failure" scenes
// what to display when they are transitioned to
public class SceneData : MonoBehaviour {
    public string currSceneZone;   // Manually set in the inspector
    public string currSceneLevel;  // Manually set in the inspector
    [HideInInspector] public int playerScore;        // Fetched from LevelStatus
    [HideInInspector] public string timeTaken;
    [HideInInspector] public bool levelPassed;

    SceneDataManager manager = null;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
        manager.currSceneZone = currSceneZone;     // TODO: These should maybe be moved to the WriteToManager() function
        manager.currSceneLevel = currSceneLevel;
    }

    public void SetScore(int score) {
        playerScore = score;
    }

    public void SetTimeTaken(float timeElapsed) {
        int secondsElapsed = Mathf.FloorToInt(timeElapsed) % 60;
        int minutesElapsed = Mathf.FloorToInt(timeElapsed) / 60;
        timeTaken = minutesElapsed.ToString() + ":" + secondsElapsed.ToString().PadLeft(2, '0');
    }

    public void SetLevelPassed(bool isPass) {
        levelPassed = isPass;
    }

    public void WriteToManager() {
        manager.levelScore = playerScore;
        manager.timeTaken = timeTaken;
        manager.levelPassed = levelPassed;
    }
}