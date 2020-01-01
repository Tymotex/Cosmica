using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStatus : MonoBehaviour {
    public float control = 50f;
    public int energy = 100;
    [SerializeField]
    [Tooltip("Don't change this from 100!")]
    float maxControl = 100f;
    [SerializeField]
    [Tooltip("This might change from level to level. Maybe the player should buy an upgrade to increase capacity")]
    int maxEnergy = 250;

    [SerializeField]
    Text energyUI = null;
    [SerializeField]
    StatusBar energyBar = null;
    [SerializeField]
    Text controlUI = null;
    [SerializeField]
    StatusBar controlBar = null;

    void Start() {
        // Display the initial energy and control percentage for the start of the current level
        UpdateUI();
    }

    private void UpdateUI() {
        energyUI.text = "Energy: " + energy.ToString();
        controlUI.text = "Control: " + control.ToString() + "%";
        energyBar.UpdateStatusBar(maxEnergy, energy);
        controlBar.UpdateStatusBar(maxControl, control);
    }

    public void AddEnergy(int amount) {
        energy += amount;
        if (energy >= maxEnergy) {
            energy = maxEnergy;
        }
        UpdateUI();
    }

    public void SpendEnergy(int amount) {
        // Debug.Log("LevelStatus: Spent " + amount.ToString() + " energy");
        energy -= amount;
        UpdateUI();
        if (energy < 0) {
            print("Energy: negative energy!");  // This should never execute. The functions calling spendEnergy() should check if there is enough energy available beforehand!
        }
    }

    public void AddControl(float amount) {
        control += amount;
        if (control <= 0) {
            Debug.Log("LevelStatus: 0 or negative control! GAME LOST!");
            // TODO: Transition to loss screen
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            sceneLoader.LoadScene("_Fail");
        } else if (control >= 100) {
            Debug.Log("LevelStatus: 100 or greater control! GAME WON!");
            // TODO: Transition to win screen
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            sceneLoader.LoadScene("_Success");
        }
        UpdateUI();
    }
}
