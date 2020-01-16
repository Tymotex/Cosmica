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
    [SerializeField] Text nextLevel = null;
    [SerializeField] Color32 greyedOutColour;

    void Start() {
        manager = FindObjectOfType<SceneDataManager>();
        if (manager != null) {  // TODO: a lot of duplicated code here
            if (manager.levelPassed) {
                title.text = "Success!"; 
                string successText = "You passed zone " + manager.currSceneZone.ToString() + " level " + manager.currSceneLevel.ToString()
                            + ".\nScore: " + manager.levelScore.ToString() + "\nTime taken: " + manager.timeTaken.ToString();
                outcomeText.text = successText;
                outcomeText.color = successColour;
            } else {
                title.text = "Critical Mission Failure!";
                string failureText = "You failed zone " + manager.currSceneZone.ToString() + " level " + manager.currSceneLevel.ToString()
                            + ".\nScore: " + manager.levelScore.ToString() + "\nTime taken: " + manager.timeTaken.ToString();
                outcomeText.text = failureText;
                outcomeText.color = failureColour;
                nextLevel.color = greyedOutColour;
                nextLevel.GetComponent<Button>().enabled = false;
            }
        }
    }
}
