using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplay : MonoBehaviour {
    CreditsManager manager = null;

    void Start() {
        manager = FindObjectOfType<CreditsManager>();
        UpdateCreditDisplay();
    }

    public void UpdateCreditDisplay() {
        manager.UpdateCreditDisplay(this);
    }

    public int GetCredits() {
        return manager.ReadCredits();
    }

    public void AddCredit(int amount) {
        manager.AddCredit(amount);
        UpdateCreditDisplay();
    }

    public void SpendCredit(int amount) {
        manager.SpendCredit(amount);
        UpdateCreditDisplay();
    }
}
