using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStatus : MonoBehaviour {
    // ===== Scene management =====
    [SerializeField]
    string successSceneName = "";
    [SerializeField]
    string failureSceneName = "";
    int currSceneIndex = 0;

    // ===== Game status variables =====
    public float control = 50f;
    public int energy = 100;
    public float elapsedTime = 0f;
    [SerializeField]
    [Tooltip("Don't change this from 100! Any other value doesn't make sense")]
    float maxControl = 100f;
    [SerializeField]
    [Tooltip("This might change from level to level. Maybe the player should buy an upgrade to increase capacity")]
    int maxEnergy = 250;
    [SerializeField]
    [Tooltip("Set the time allowed for this level in seconds (ignoring overtime). 300 gives the player 5 minutes to beat the level")]
    float maxTime = 300;

    // ===== Status UI =====
    [SerializeField]
    Text energyUI = null;
    [SerializeField]
    StatusBar energyBar = null;
    //[SerializeField]  // The parent gameobject of the energy text and energy bar. This is for animating the pulse animation when energy is gained/spent
    //GameObject energyUIContainer = null;
    [SerializeField]
    Text controlUI = null;
    [SerializeField]
    StatusBar controlBar = null;
    //[SerializeField]  // The parent gameobject of the control text and control bar
    //GameObject controlUIContainer = null;
    [SerializeField]
    Text timerUI = null;
    [SerializeField]
    StatusBar timerBar = null;
    [SerializeField]
    Color overtimeWarningColour = Color.yellow;
    //[SerializeField]  // The parent gameobject of the timer text and timer bar
    //GameObject timerUIContainer = null;

    // ===== Level End Animations =====
    [SerializeField]
    [Tooltip("Make this at least as long as how long the level completion animations last for")]
    float timeBeforeTransition = 1f;
    [SerializeField]
    Text levelCompletionText = null;
    [SerializeField]
    string levelWinText = "Level cleared!";
    [SerializeField]
    string levelFailText = "Level failed!";
    [SerializeField]
    GameObject levelCompleteUI = null;
    [SerializeField]
    GameObject selectionBarUI = null;
    //[SerializeField]
    //GameObject bannerBackground = null;
    //[SerializeField]
    //GameObject fadeBackground = null;


    void Start() {
        // Display the initial energy and control percentage for the start of the current level
        UpdateUI();
        // Set the current scene index
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;  // Gets the currently active scene's index (which is set in the build settings)
    }

    void Update() {
        elapsedTime += Time.deltaTime;  // Keeps track of REAL time from the start of the level. Time.deltaTime returns the time in seconds since the last frame update (so summing all deltaTimes gives the total time elapsed)
        bool timeExceeded = elapsedTime > maxTime;
        UpdateTimer(timeExceeded);
        // TODO: Make sure the if statement is only entered once! The shit inside here will get executed every frame update!
        if (timeExceeded) {
            timerUI.color = overtimeWarningColour;
            HaltEnemySpawning();
            if (!EnemiesStillPresent()) {
                // The player passes the level if they reached a control level of at least 50%:
                if (control >= maxControl / 2) {
                    StartCoroutine(EndLevel(true));
                }
                // The player fails the level if they had <50% control over the level
                else {
                    StartCoroutine(EndLevel(false));
                }
            }
        }
    }

    private void UpdateUI() {
        energyUI.text = "Energy: " + energy.ToString();
        controlUI.text = "Control: " + control.ToString() + "%";

        energyBar.UpdateStatusBar(maxEnergy, energy);
        controlBar.UpdateStatusBar(maxControl, control);
    }

    private void UpdateTimer(bool timeExceeded) {
        int secondsElapsed = Mathf.FloorToInt(elapsedTime) % 60;
        int minutesElapsed = Mathf.FloorToInt(elapsedTime) / 60;
        if (timeExceeded) {
            timerUI.text = "OVERTIME: " + minutesElapsed.ToString() + ":" + secondsElapsed.ToString().PadLeft(2, '0');
        } else {
            timerUI.text = "Time: " + minutesElapsed.ToString() + ":" + secondsElapsed.ToString().PadLeft(2, '0');
            timerBar.UpdateStatusBar(maxTime, elapsedTime);
        }
    }

    public void AddEnergy(int amount) {
        /*
        Animator energyUIAnimator = energyUIContainer.GetComponent<Animator>();
        energyUIAnimator.SetTrigger("energyChanged");
        AnimationClip[] animations = energyUIAnimator.runtimeAnimatorController.animationClips;
        // Get the length of the pulse animation clip and reset the trigger after this time has elapsed. This allows the clip to be playable at any point during runtime
        foreach (AnimationClip clip in animations) {
            float animationTime;
            if (clip.name == "EnergyBarPulse") {
                animationTime = clip.length;
                StartCoroutine(resetTrigger(energyUIAnimator, "energyChanged", animationTime));
            }
        }
        */
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
            HaltEnemySpawning();
            StartCoroutine(EndLevel(false));
        } else if (control >= 100) {
            Debug.Log("LevelStatus: 100 or greater control! GAME WON!");
            // TODO: Transition to win screen
            HaltEnemySpawning();
            StartCoroutine(EndLevel(true));
        }
        UpdateUI();
    }

    private void HaltEnemySpawning() {
        // Halt all enemy spawners
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners) {
            spawner.spawning = false;
        }
    }

    private IEnumerator EndLevel(bool levelPassed) {
        Animator levelCompleteUIAnimator = levelCompleteUI.GetComponent<Animator>();
        Animator selectionBarUIAnimator = selectionBarUI.GetComponent<Animator>();
        Animator statusBarAnimator = gameObject.GetComponent<Animator>();
        if (levelPassed) {
            levelCompletionText.text = levelWinText;
        } else {
            levelCompletionText.text = levelFailText;
        }
        // levelCompleted is a parameter for the animator controller state machine. Setting it to true triggers any animations meant for level completion
        levelCompleteUIAnimator.SetBool("levelCompleted", true);
        selectionBarUIAnimator.SetBool("levelCompleted", true);
        statusBarAnimator.SetBool("levelCompleted", true);
        yield return new WaitForSeconds(timeBeforeTransition);

        if (levelPassed) {
            LoadScene(successSceneName);
        } else {
            LoadScene(failureSceneName);
        }
    }

    private bool EnemiesStillPresent() {
        bool enemyPresent = false;
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners) {
            if (spawner.EnemyExistsInRow()) {
                enemyPresent = true;
                break;
            }
        }
        return enemyPresent;
    }

    private void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    /*
    private IEnumerator resetTrigger(Animator animator, string triggerName, float waitDuration) {
        yield return new WaitForSeconds(waitDuration);
        animator.ResetTrigger(triggerName);
    }
    */
}

