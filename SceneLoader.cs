using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour {
    [SerializeField] 
    int transitionDelay;   // Time taken by the splash screen
    
    int currSceneIndex;

    // Start is called before the first frame update
    void Start() {
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;  // Gets the currently active scene's index (which is set in the build settings)
        if (currSceneIndex == 0) {    // If the current scene is the splash screen, then sleep for a few seconds before transitioning
            StartCoroutine(Sleep());
        }
    }

    IEnumerator Sleep() {

        yield return new WaitForSeconds(transitionDelay);
        LoadNextScene();
    }

    public void LoadNextScene() {
        SceneManager.LoadScene(currSceneIndex + 1);  // Load the next scene index
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update() {
        
    }

    
}
