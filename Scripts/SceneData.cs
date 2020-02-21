using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// Manually set the fields in the inspector. The fields tell the "success" and "failure" scenes
// what to display when they are transitioned to
public class SceneData : MonoBehaviour {
    // ===== For Outcome Writer =====
    public string currSceneZone;    // Manually set in the inspector
    public string currSceneLevel;   // Manually set in the inspector
    int playerScore;                // Fetched from LevelStatus
    string timeTaken;
    bool levelPassed;
    int controlAttained;
    int controlBonus;
    int timeBonus;
    int energySpent;
    int energyPenalty;
    [Tooltip("How much credit the player earns on passing this level")]
    public int baseCreditReward;
    Defender shipUnlockReward;

    // ===== Other Links =====
    [SerializeField] Text headerText = null;
    SceneDataManager manager = null;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
        // TODO: fix up crude code and magic numbers. Levelx_y
        string sceneName = SceneManager.GetActiveScene().name;
        currSceneZone = sceneName[5].ToString();
        if (sceneName.Length > 8) {
            currSceneLevel = sceneName[7].ToString() + sceneName[8].ToString();
        } else {
            currSceneLevel = sceneName[7].ToString();
        }
        Debug.Log("SCENEDATA: zone " + currSceneZone + " level " + currSceneLevel);
        manager.currSceneZone = currSceneZone;     // TODO: These should maybe be moved to the WriteToManager() function
        manager.currSceneLevel = currSceneLevel;
        headerText.text = "Zone " + currSceneZone.ToString() + " Level " + currSceneLevel.ToString();
        SetLevelRewards();
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
        if (!isPass) {  // If failed, then don't give the player any base rewards for passing the level
            manager.baseCreditReward = 0;
            manager.unlockedShip = null;
        }
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

    private void SetLevelRewards() {
        int nextLevel;  // TODO: Similar code chunk present 3 times in this file
        int nextZone;
        if (int.Parse(currSceneLevel) >= 10) {
            nextLevel = 1;
            nextZone = int.Parse(currSceneZone) + 1;
        } else {
            nextLevel = int.Parse(currSceneLevel) + 1;
            nextZone = int.Parse(currSceneZone);
        }
        Debug.Log("Next level is ZONE: " + nextZone + " LEVEL: " + nextLevel);
        if (!PlayerData.LevelIsUnlocked(nextZone, nextLevel)) {
            Debug.Log("HAS NOT BEEN UNLOCKED! Allowing rewards");
            manager.baseCreditReward = baseCreditReward;
            shipUnlockReward = GetNextLevelShipUnlock();
            manager.unlockedShip = shipUnlockReward;
        } else {
            manager.baseCreditReward = 0;
            manager.unlockedShip = null;
        }
    }

    // Finds out if there is a new defender rewarded on completing this level
    private Defender GetNextLevelShipUnlock() {
        int nextLevel;
        int nextZone;
        if (int.Parse(currSceneLevel) >= 10) {
            nextLevel = 1;
            nextZone = int.Parse(currSceneZone) + 1;
        } else {
            nextLevel = int.Parse(currSceneLevel) + 1;
            nextZone = int.Parse(currSceneZone);
        }
        // Find the next ship to be unlocked, if such one exists
        Selector[] allShips = FindObjectsOfType<Selector>();
        foreach (Selector ship in allShips) {
            if (ship.levelForUnlock == nextLevel && ship.zoneForUnlock == nextZone) {
                Debug.Log("===> Ship is unlocked next level!!! " + ship.defenderPrefab);
                return ship.defenderPrefab;
            }
        }
        return null;
    }
}