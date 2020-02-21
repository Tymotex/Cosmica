using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {
    AsyncOperation loadOperation;
    [SerializeField] Slider loadingBar;
    [SerializeField] Text loadingPercentage;
    [SerializeField] float screenShowupDelay;

    void Start() {
        StartCoroutine(LoadStartScene());
    }

    private IEnumerator LoadStartScene() {
        yield return new WaitForSeconds(screenShowupDelay);
        loadOperation = SceneManager.LoadSceneAsync("_Start");
        while (!loadOperation.isDone) {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);  // Loading occurs in the progress interval 0.0 to 0.9 while activation occurs in the interval 0.9 to 1.0. The isDone property is true when 0.9 is reached
            loadingBar.value = progress;
            loadingPercentage.text = Mathf.FloorToInt(progress * 100) + "%";
            yield return null;  // Waits a frame
        }
    }
}
