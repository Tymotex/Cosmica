using UnityEngine;
using System.Collections;

// Collects data from 'SceneData' objects, if present. 
// SceneData objects will be present in gameplay scenes, like level01_01
public class SceneDataManager : MonoBehaviour {
    // Persistent 'global' object
    public static SceneDataManager uniqueDataManager;
    
    // ===== Scene data =====
    public string currSceneZone;
    public string currSceneLevel;
    public int levelScore;
    public string timeTaken;       // Formatted string in minutes:seconds (eg. 4:30)
    public bool levelPassed;

    // ===== Score Keeping =====
    public int controlAttained;
    public int controlBonus;
    public int timeBonus;
    public int energySpent;
    public int energyPenalty;

    void Awake() {
        if (uniqueDataManager != null) {
            if (uniqueDataManager != this) {
                Destroy(gameObject);
            }
        } else {
            uniqueDataManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PrintManagerData() {
        Debug.Log("Level passed: " + levelPassed);
        Debug.Log("Curr level  : " + currSceneLevel);
        Debug.Log("Time taken  : " + timeTaken);
    }
}