using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO: Give a better classname
public class OutcomeWriter : MonoBehaviour {
    SceneDataManager manager = null;
    [SerializeField] Text title = null;  // TODO: Maybe make a different animation for this title
    [SerializeField] Color32 successColour;   
    [SerializeField] Color32 failureColour;
    [SerializeField] Text outcomeText = null;
    [SerializeField] Text outcomeStats = null;
    [SerializeField] Button nextLevelButton = null;
    [SerializeField] Color32 greyedOutColour;
    [SerializeField] Text scoreValuesText = null;
    [SerializeField] Text rewardValuesText = null;
    [SerializeField] Image unlockedShip = null;
    [SerializeField] Color unlockShipColour;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
        manager.PrintManagerData();
        if (manager != null) {  // TODO: a lot of duplicated code here
            if (manager.levelPassed) {
                title.text = "Mission Success"; 
                string successText = "You passed zone " + manager.currSceneZone.ToString() + " level " + manager.currSceneLevel.ToString();
                outcomeText.text = successText;
                outcomeText.color = successColour;
            } else {
                title.text = "Critical Mission Failure";
                string failureText = "You failed zone " + manager.currSceneZone.ToString() + " level " + manager.currSceneLevel.ToString();
                outcomeText.text = failureText;
                outcomeText.color = failureColour;
                nextLevelButton.GetComponent<Image>().color = greyedOutColour;
                nextLevelButton.GetComponent<Button>().enabled = false;
            }
            string statsText = manager.controlAttained.ToString() + "%" +
                "\n+" + manager.controlBonus.ToString() +
                "\n" + manager.timeTaken.ToString() +
                "\n+" + manager.timeBonus.ToString() +
                "\n" + manager.energySpent.ToString() + 
                "\n-" + manager.energyPenalty.ToString() +
                "\n" + manager.levelScore.ToString();
            outcomeStats.text = statsText;
            // Write to the scores panel
            int zone = int.Parse(manager.currSceneZone);
            int level = int.Parse(manager.currSceneLevel);
            // Set new high score if greater than last high score
            int lastHighScore = PlayerData.GetHighScore(zone, level);
            int levelScore = manager.levelScore;
            if (levelScore > lastHighScore) {
                PlayerData.SetHighScore(zone, level, levelScore);
                lastHighScore = levelScore;
            }
            string scoresText = levelScore.ToString() +
                "\n" + lastHighScore.ToString();
            scoreValuesText.text = scoresText;
            // Write to the rewards panel AND add them to PlayerData
            int earnedCredit = manager.baseCreditReward + Mathf.FloorToInt(levelScore * manager.creditToScoreRatio);
            string unlockedShipName = "";
            if (manager.unlockedShip != null) {
                unlockedShipName = manager.unlockedShip.defenderUnit.defenderName;
                unlockedShip.sprite = manager.unlockedShip.defenderUnit.GetComponent<SpriteRenderer>().sprite;
                unlockedShip.color = unlockShipColour;
            }
            rewardValuesText.text = "+" + earnedCredit.ToString() +
                "\n" + unlockedShipName;
            
            FindObjectOfType<CreditsManager>().AddCredit(earnedCredit);
        }
    }
}
