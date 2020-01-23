using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStatus : MonoBehaviour {
    // ===== Scene management =====
    [SerializeField] string levelOutcomeSceneName = null;
    [SerializeField] SceneData sceneData = null;

    // ===== Game status variables =====
    public float control = 50f;
    public int energy = 100;
    public float elapsedTime = 0f;
    public float timeSinceLastRampUp = 0f;
    [SerializeField] float rampUpInterval = 30;   // Difficulty heightens every 30 seconds
    [SerializeField]
    [Tooltip("Don't change this from 100! Any other value doesn't make sense")]
    float maxControl = 100f;
    [SerializeField]
    [Tooltip("This might change from level to level. Maybe the player should buy an upgrade to increase capacity")]
    int maxEnergy = 250;
    [SerializeField]
    [Tooltip("Set the time allowed for this level in seconds (ignoring overtime). 300 gives the player 5 minutes to beat the level")]
    float maxTime = 300;
    int score = 100;

    // ===== Status UI =====
    [SerializeField] Text energyUI = null;
    [SerializeField] StatusBar energyBar = null;
    [SerializeField] Text controlUI = null;
    [SerializeField] StatusBar controlBar = null;
    [SerializeField] Text timerUI = null;
    [SerializeField] StatusBar timerBar = null;
    [SerializeField] Color overtimeWarningColour = Color.yellow;

    // ===== Level End =====
    [SerializeField]
    [Tooltip("Make this at least as long as how long the level completion animations last for")]
    float timeBeforeTransition = 1f;
    [SerializeField] Text levelCompletionText = null;
    [SerializeField] string levelWinText = "Level cleared!";
    [SerializeField] string levelFailText = "Level failed!";
    [SerializeField] GameObject levelCompleteUI = null;
    [SerializeField] GameObject selectionBarUI = null;
    [SerializeField] Canvas headerCanvas = null;
    [SerializeField] SoundClip levelSuccessSFX = null;
    [SerializeField] SoundClip levelFailSFX = null;
    bool isOvertime = false;
    public bool endingLevel = false;

    // ===== Ramping Up Difficulty =====
    public EnemySpawner[] enemySpawners = null;

    // ===== Preparation Phase =====
    public bool levelStarted = false;
    [SerializeField] GameObject prepPhaseUI = null;

    void Start() {
        // Display the initial energy and control percentage for the start of the current level
        UpdateUI();
        // Initialise the enemy spawners array
        enemySpawners = FindObjectsOfType<EnemySpawner>();

        GameObject prepPhase = Instantiate(prepPhaseUI, Vector3.zero, Quaternion.identity) as GameObject;
        prepPhase.transform.SetParent(GameObject.FindGameObjectWithTag("HeaderCanvas").transform, false);
        prepPhase.transform.position = transform.position;
        foreach (EnemySpawner spawner in enemySpawners) {  // TODO: Unnecessary?
            spawner.spawning = false;
        }
    }

    void Update() {
        if (levelStarted) {
            elapsedTime += Time.deltaTime;  // Keeps track of REAL time from the start of the level. Time.deltaTime returns the time in seconds since the last frame update (so summing all deltaTimes gives the total time elapsed)
            timeSinceLastRampUp += Time.deltaTime;
            if (timeSinceLastRampUp >= rampUpInterval && elapsedTime <= maxTime) {  // Ramp up difficulty every x seconds until overtime is reached
                timeSinceLastRampUp -= rampUpInterval;
                // Debug.Log("Ramping up difficulty!");
                foreach (EnemySpawner spawner in enemySpawners) {
                    spawner.rampIndex++;
                }
            }
            bool timeExceeded = elapsedTime > maxTime;
            UpdateTimer(timeExceeded);
            if (timeExceeded && !isOvertime) {
                isOvertime = true;
                timerUI.color = overtimeWarningColour;
                HaltEnemySpawning();
            }
            if (isOvertime) {
                if (!EnemiesStillPresent()) {
                    if (control >= maxControl / 2) {     // The player passes the level if they reached a control level of at least 50%:
                        if (!endingLevel) {
                            endingLevel = true;
                            StartCoroutine(EndLevel(true));
                        }
                    } else {                               // The player fails the level if they had <50% control over the level
                        if (!endingLevel) {
                            endingLevel = true;
                            StartCoroutine(EndLevel(false));
                        }
                    }
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
        energy += amount;
        if (energy >= maxEnergy) {
            energy = maxEnergy;
        }
        UpdateUI();
    }

    public void SpendEnergy(int amount) {
        energy -= amount;
        UpdateUI();
        if (energy < 0) {
            energy = 0;
            Debug.Log("Energy: negative energy!");  // This should never execute. The functions calling spendEnergy() should check if there is enough energy available beforehand!
        }
    }

    public void AddControl(float amount) {
        control += amount;
        if (control <= 0) {
            control = 0;
            if (!endingLevel) {
                endingLevel = true;
                Debug.Log("LevelStatus: 0 or negative control! GAME LOST!");
                HaltEnemySpawning();
                StartCoroutine(EndLevel(false));
            }
        } else if (control >= 100 && !endingLevel) {
            control = 100;
            if (!endingLevel) {
                endingLevel = true;
                Debug.Log("LevelStatus: 100 or greater control! GAME WON!");
                HaltEnemySpawning();
                StartCoroutine(EndLevel(true));
            }
        }
        UpdateUI();
    }

    private void HaltEnemySpawning() {
        // Halt all enemy spawners
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners) {
            spawner.ForceStopSpawning();
        }
    }

    private IEnumerator EndLevel(bool levelPassed) {
        Animator levelCompleteUIAnimator = levelCompleteUI.GetComponent<Animator>();
        Animator selectionBarUIAnimator = selectionBarUI.GetComponent<Animator>();
        Animator statusBarAnimator = gameObject.GetComponent<Animator>();
        Animator headerCanvasAnimator = headerCanvas.GetComponent<Animator>();
        AudioSource audioSource = GetComponent<AudioSource>();
        if (levelPassed) {
            levelCompletionText.text = levelWinText;
            audioSource.clip = levelSuccessSFX.clip;
            audioSource.volume = levelSuccessSFX.volume;
            audioSource.pitch = levelSuccessSFX.pitch;
            audioSource.Play();
            // Call end of level functions
            Defender[] defenders = FindObjectsOfType<Defender>();
            foreach (Defender defender in defenders) {
                defender.PlayVictoryAnimation();
                defender.defenderUnit.StopShooting();
            }
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies) {
                enemy.PlayDefeatAnimation();
                enemy.enemyUnit.StopShooting();
            }
            EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
            foreach (EnemySpawner spawner in spawners) {
                spawner.ForceStopSpawning();
            }
            Projectile[] projectiles = FindObjectsOfType<Projectile>();
            foreach (Projectile projectile in projectiles) {
                projectile.Dissolve();
            }
        } else {
            levelCompletionText.text = levelFailText;
            Debug.Log("Playing fail SFX");
            audioSource.clip = levelFailSFX.clip;
            audioSource.volume = levelFailSFX.volume;
            audioSource.pitch = levelFailSFX.pitch;
            audioSource.Play();
        }
        // Triggering level end animations:
        // levelCompleted is a parameter for the animator controller state machine. Setting it to true triggers any animations meant for level completion
        levelCompleteUIAnimator.SetBool("levelCompleted", true);
        selectionBarUIAnimator.SetBool("levelCompleted", true);
        statusBarAnimator.SetBool("levelCompleted", true);
        headerCanvasAnimator.SetBool("levelCompleted", true);
        yield return new WaitForSeconds(timeBeforeTransition);
        // Update the final score the player achieved and the time taken in the SceneData object
        sceneData.SetScore(score);
        sceneData.SetTimeTaken(elapsedTime);
        sceneData.SetLevelPassed(levelPassed);
        sceneData.WriteToManager();
        // Finally transition to the outcome scene
        LoadScene(levelOutcomeSceneName);
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
}

