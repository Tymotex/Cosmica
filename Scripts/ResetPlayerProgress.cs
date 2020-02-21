using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetPlayerProgress : MonoBehaviour {
    [SerializeField] GameObject confirmPanelPrefab = null;
    GameObject confirmationPanel;

    public void PromptConfirm() {
        confirmationPanel = Instantiate(confirmPanelPrefab, transform.position, Quaternion.identity) as GameObject;
    }
    
    public void WipeAllProgress() {
        Debug.Log("Resetting all prefs");
        PlayerPrefs.DeleteAll();
        Destroy(gameObject);
        SceneManager.LoadScene("_Options");
    }

    public void CancelConfirm() {
        Destroy(gameObject);
    }
}
