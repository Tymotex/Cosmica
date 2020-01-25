using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {
    [SerializeField] Text creditText = null;
    public static CreditsManager uniqueCreditsManager;

    void Awake() {
        if (uniqueCreditsManager != null) {
            if (uniqueCreditsManager != this) {
                Destroy(gameObject); 
            }
        } else {
            uniqueCreditsManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void UpdateCreditDisplay(CreditsDisplay creditsDisplay) {
        creditsDisplay.GetComponent<Text>().text = PlayerData.GetCredits().ToString();
    }

    public void AddCredit(int amount) {
        PlayerData.SetCredits(PlayerData.GetCredits() + amount);
    }

    public void SpendCredit(int amount) {
        PlayerData.SetCredits(PlayerData.GetCredits() - amount);
    }
}
