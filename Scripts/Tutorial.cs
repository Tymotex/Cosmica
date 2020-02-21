using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    [SerializeField] GameObject[] tutorialPanels = null;
    bool[] tutorialPassed;
    int tutIndex = 0;
    PreparationPhase prepPhaseUI;
    bool prepUIIsOn = false;
    DefenderTile[] tiles;
    GameObject currPanel;
    LevelStatus levelStatus;

    void Start() {
        levelStatus = FindObjectOfType<LevelStatus>();
        prepPhaseUI = FindObjectOfType<PreparationPhase>();
        TogglePrepPhaseUI();
        LoadPanel(tutIndex);
        tutorialPassed = new bool[tutorialPanels.Length];
        tiles = FindObjectsOfType<DefenderTile>();
        for (int i = 0; i < tutorialPanels.Length; i++) {
            tutorialPassed[i] = false;
        }
    }

    private void LoadPanel(int panelIndex) {
        currPanel = Instantiate(tutorialPanels[panelIndex], transform.position, Quaternion.identity) as GameObject;
        currPanel.transform.SetParent(transform);
        currPanel.transform.localScale = Vector3.one;
        currPanel.transform.localPosition += currPanel.GetComponent<TutorialPanel>().panelOffset;
    }

    private void DisableCurrentPanel() {
        currPanel.SetActive(false);
    }

    public void ProgressToNextTutorial() {
        tutIndex++;
        Destroy(currPanel);
        LoadPanel(tutIndex);
    }

    private void Update() {
        switch (tutIndex) {
            case (0):
                Debug.Log("Tutorial 1");
                // Player needs to have placed down a ship to be able to progress to the next tutorial panel
                if (!tutorialPassed[tutIndex]) {
                    foreach (DefenderTile tile in tiles) {
                        if (tile.DefenderIsPresent()) {
                            tutorialPassed[tutIndex] = true;
                            ProgressToNextTutorial();
                        }
                    }
                }
                break;
            case (1):
                Debug.Log("Tutorial 2");
                break;
            case (2):
                Debug.Log("Tutorial 3");
                break;
            case (3):  // Preparation phase
                Debug.Log("Tutorial 4");
                if (!prepUIIsOn) {
                    prepUIIsOn = true;
                    TogglePrepPhaseUI();
                }
                if (levelStatus.levelStarted) {
                    ProgressToNextTutorial();
                }
                break;
            case (4):
                Debug.Log("Tutorial 5");
                break;
            case (5):
                Debug.Log("End tutorial");
                if (levelStatus.endingLevel) {
                    DisableCurrentPanel();
                }
                break;
        }
    }

    private void TogglePrepPhaseUI() {
        if (prepPhaseUI.gameObject.activeSelf) {
            prepPhaseUI.gameObject.SetActive(false);
        } else {
            prepPhaseUI.gameObject.SetActive(true);
        }
    }
}
