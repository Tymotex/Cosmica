using UnityEngine;
using System.Collections;

// Collects data from 'SceneData' objects, if present. 
// SceneData objects will be present in gameplay scenes, like level01_01
public class SceneDataManager : MonoBehaviour {
    // Persistent 'global' object
    public static SceneDataManager Instance { get; set; }
    
    // ===== Scene data =====
    public string currSceneZone;
    public string currSceneLevel;
    public int levelScore;
    public string timeTaken;       // Formatted string in minutes:seconds (eg. 4:30)
    public bool levelPassed;

    // TODO: SceneManager duplicates!
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    // FOR DEBUGGING
    void Update() {
        /*
        Debug.Log("===> zone: " + currSceneZone);
        Debug.Log("===> levl: " + currSceneLevel);
        Debug.Log("===> scor: " + levelScore);
        Debug.Log("===> time: " + timeTaken);
        Debug.Log("===> pass: " + levelPassed);
        */
    }
}