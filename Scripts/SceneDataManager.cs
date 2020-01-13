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
    public int timeTaken;

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }
}