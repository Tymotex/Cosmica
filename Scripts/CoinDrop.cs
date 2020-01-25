using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDrop : MonoBehaviour {
    [SerializeField] int coinWorth = 15;
    [SerializeField] Text coinText = null;

    void Start() {
        CreditsDisplay creditDisplayer = FindObjectOfType<CreditsDisplay>();
        creditDisplayer.UpdateCreditDisplay();
        creditDisplayer.AddCredit(coinWorth);
        coinText.text = "+" + coinWorth.ToString();
    }
}
