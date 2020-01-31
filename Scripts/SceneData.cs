using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Manually set the fields in the inspector. The fields tell the "success" and "failure" scenes
// what to display when they are transitioned to
public class SceneData : MonoBehaviour {
    public string currSceneZone;   // Manually set in the inspector
    public string currSceneLevel;  // Manually set in the inspector
    [HideInInspector] public int playerScore;        // Fetched from LevelStatus
    [HideInInspector] public string timeTaken;
    [HideInInspector] public bool levelPassed;
    int controlAttained;
    int controlBonus;
    int timeBonus;
    int energySpent;
    int energyPenalty;
    [SerializeField] Text headerText = null;

    SceneDataManager manager = null;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
        manager.currSceneZone = currSceneZone;     // TODO: These should maybe be moved to the WriteToManager() function
        manager.currSceneLevel = currSceneLevel;
        headerText.text = "Zone " + currSceneZone.ToString() + " Level " + currSceneLevel.ToString();
    }

    public void SetScore(int score) {
        playerScore = score;
    }

    public void SetTimeTaken(float timeElapsed) {
        int secondsElapsed = Mathf.FloorToInt(timeElapsed) % 60;
        int minutesElapsed = Mathf.FloorToInt(timeElapsed) / 60;
        timeTaken = minutesElapsed.ToString() + ":" + secondsElapsed.ToString().PadLeft(2, '0');
    }

    public void SetControlAttained(int controlAttained) {
        this.controlAttained = controlAttained;
    }

    public void SetLevelPassed(bool isPass) {
        levelPassed = isPass;
    }

    public void SetScoreBonuses(int controlBonus, int timeBonus, int energyPenalty) {
        this.controlBonus = controlBonus;
        this.timeBonus = timeBonus;
        this.energyPenalty = energyPenalty;
    }

    public void SetEnergySpent(int energySpent) {
        this.energySpent = energySpent;
    }

    public void WriteToManager() {
        manager.levelScore = playerScore;
        manager.timeTaken = timeTaken;
        manager.levelPassed = levelPassed;
        manager.controlAttained = controlAttained;
        manager.controlBonus = controlBonus;
        manager.timeBonus = timeBonus;
        manager.energySpent = energySpent;
        manager.energyPenalty = energyPenalty;
    }

    // TODO: What if we've reached the last level?
    public void UnlockNextLevel() {
        string nextLevel;
        string nextZone;
        if (int.Parse(currSceneLevel) >= 10) {
            nextLevel = "1";
            nextZone = (int.Parse(currSceneZone) + 1).ToString();
        } else {
            nextLevel = (int.Parse(currSceneLevel) + 1).ToString();
            nextZone = currSceneZone;
        }
        string levelName = "Level" + nextZone.ToString() + "_" + nextLevel.ToString();
        PlayerData.UnlockLevel(levelName);
    }
}