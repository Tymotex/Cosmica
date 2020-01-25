using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDisplay : MonoBehaviour {
    CreditsManager manager = null;

    void Awake() {
        manager = FindObjectOfType<CreditsManager>();
        UpdateCreditDisplay();
    }

    public void UpdateCreditDisplay() {
        manager.UpdateCreditDisplay(this);
    }

    public void AddCredit(int amount) {
        manager.AddCredit(amount);
    } 
}
