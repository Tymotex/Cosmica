using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeWriter : MonoBehaviour {
    SceneDataManager manager = null;
    [SerializeField] Text title = null;  // TODO: Maybe make a different animation for this title
    [SerializeField] Color32 successColour;   
    [SerializeField] Color32 failureColour;
    [SerializeField] Text outcomeText = null;
    [SerializeField] Text outcomeStats = null;
    [SerializeField] Button nextLevelButton = null;
    [SerializeField] Color32 greyedOutColour;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
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
            string statsText = "Score: " + manager.levelScore.ToString() + "\nTime: " + manager.timeTaken.ToString();
            outcomeStats.text = statsText;
        }
    }
}
