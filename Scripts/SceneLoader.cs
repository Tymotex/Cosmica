using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    int currSceneIndex;
    [SerializeField]
    GameObject fadeBackground = null;
    [SerializeField]
    float transitionDelay = 1;

    // Start is called before the first frame update
    void Start() {
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;  // Gets the currently active scene's index (which is set in the build settings)
        /*
        if (currSceneIndex == 0) {    // If the current scene is the splash screen, then sleep for a few seconds before transitioning
            StartCoroutine(Sleep());
        }
        */
    }

    private string FormatLevelName(SceneDataManager manager, bool loadNextLevel) {
        if (loadNextLevel) {
            return "Level" + manager.currSceneZone.ToString() + "_" + (System.Convert.ToInt32(manager.currSceneLevel) + 1).ToString();
        } else {
            return "Level" + manager.currSceneZone.ToString() + "_" + manager.currSceneLevel.ToString();
        }
    }

    public void ReloadLastLevel() {
        fadeOut();
        SceneDataManager manager = FindObjectOfType<SceneDataManager>();
        string lastLevel = FormatLevelName(manager, false);
        Debug.Log("Loading last level: " + lastLevel);
        StartCoroutine(LoadAfterDelay(transitionDelay, lastLevel));
    }

    public void LoadNextLevel() {
        fadeOut();
        SceneDataManager manager = FindObjectOfType<SceneDataManager>();
        string nextLevel = FormatLevelName(manager, true);
        Debug.Log("Loading next level: " + nextLevel);
        StartCoroutine(LoadAfterDelay(transitionDelay, nextLevel));
    }

    public void LoadNextScene() {
        fadeOut();
        StartCoroutine(LoadAfterDelay(transitionDelay, currSceneIndex + 1));
    }

    public void LoadPreviousScene() {
        fadeOut();
        StartCoroutine(LoadAfterDelay(transitionDelay, currSceneIndex - 1));
    }

    public void LoadScene(string sceneName) {
        fadeOut();
        StartCoroutine(LoadAfterDelay(transitionDelay, sceneName));
    }

    private IEnumerator LoadAfterDelay(float transitionDelay, string sceneName) {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadAfterDelay(float transitionDelay, int sceneIndex) {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex);
    }

    private void fadeOut() {
        Animator fadeAnimator = fadeBackground.GetComponent<Animator>();
        fadeAnimator.SetBool("transitioning", true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
