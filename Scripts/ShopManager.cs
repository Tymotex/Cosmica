using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    [HideInInspector] public Defender currentDefender;
    [HideInInspector] public Defender[] currentDefenderTiers;
    public ShopSelector[] selectors;
    public GameObject[] tierPanels;
    public Text[] tierStatsTexts;
    public Button buyButton;
    public Color32 greyedOutColour;
    public int maxTier = 3;
    public Text[] priceText;
    public Image[] tierSprites;
    public CreditsDisplay creditsDisplay;  // Handles the spending of credits by talking to CreditsManager

    void Start() {
        ClearPanels();
    }

    private void ClearPanels() {
        currentDefender = null;
        currentDefenderTiers = null;
        // Grey-out the panels
        foreach (GameObject panel in tierPanels) {
            foreach (Transform child in panel.transform) {
                Image imgComponent = child.GetComponent<Image>();
                Text txtComponent = child.GetComponent<Text>();
                if (txtComponent != null) {
                    txtComponent.color = Color.clear;
                }
                if (imgComponent != null) {
                    imgComponent.color = greyedOutColour;
                }
            }
        }
        // Clear tier sprites
        for (int i = 0; i < maxTier; i++) {
            tierSprites[i].GetComponent<Image>().color = Color.clear;
        }
        // Grey-out the button
        Text buttonTxtComp = buyButton.transform.GetComponentInChildren<Text>();
        buttonTxtComp.text = "Upgrade";
        buttonTxtComp.color = greyedOutColour;
        buyButton.enabled = false;
    }

    private void UpdatePanels() {
        int i = 0;
        int currentTier = PlayerData.GetShipTier(currentDefender.shipFamily);
        // Update stats text
        foreach (Text tierStatsText in tierStatsTexts) {
            DefenderBehaviour unit = currentDefenderTiers[i].defenderUnit;
            float avgDamage = (unit.ammo.minDamage + unit.ammo.maxDamage) / 2f;
            Debug.Log("Avg Damange: " + unit.ammo.minDamage + " and " + unit.ammo.maxDamage + " = " + avgDamage);
            float avgFirerate = 1 / ((unit.minShootDelay + unit.maxShootDelay) / 2);
            float avgDPS = avgDamage * avgFirerate;
            tierStatsText.text = unit.defenderName +
                "\n" + unit.costToSpawn +
                "\n" + avgDamage.ToString("0.##") +
                "\n" + avgFirerate.ToString("0.##") + "/sec" +
                "\n" + avgDPS.ToString("0.##") +
                "\n" + unit.GetComponent<Health>().maxHealth +
                "\n" + unit.GetComponent<Health>().defence;
            i++;
        }
        // Grey-out the panels
        i = 0;
        foreach (GameObject tierPanel in tierPanels) {
            Debug.Log("Current tier is: " + currentTier);  // TODO: Repetitive code
            if (currentTier != (i + 1)) {
                foreach (Transform child in tierPanel.transform) {
                    Image imgComponent = child.GetComponent<Image>();
                    Text txtComponent = child.GetComponent<Text>();
                    if (imgComponent != null) {
                        imgComponent.color = greyedOutColour;
                    }
                    if (txtComponent != null) {
                        txtComponent.color = greyedOutColour;
                    }
                }
            } else {
                foreach (Transform child in tierPanel.transform) {
                    Image imgComponent = child.GetComponent<Image>();
                    Text txtComponent = child.GetComponent<Text>();
                    if (imgComponent != null) {
                        imgComponent.color = Color.white;
                    }
                    if (txtComponent != null) {
                        txtComponent.color = Color.white;
                    }
                }
            }
            i++;
        }
        // Update the upgrade prices text
        i = 0;
        for (int j = 1; j < maxTier; j++, i++) {
            string cost = currentDefenderTiers[j].costToUpgrade.ToString();
            priceText[i].text = "$" + cost;
        }
        // Update tier sprites
        for (int j = 0; j < maxTier; j++) {
            tierSprites[j].sprite = currentDefenderTiers[j].defenderUnit.GetComponent<SpriteRenderer>().sprite;
        }
        // Update the button text. Grey-out the button if the ship is already at its maximum tier
        Text buttonTxtComp = buyButton.transform.GetComponentInChildren<Text>();
        string nextTier = " ";
        if (currentTier >= maxTier) {
            buyButton.enabled = false;
            buttonTxtComp.color = greyedOutColour;
            buttonTxtComp.text = "No More Upgrades";
        } else {
            nextTier += currentDefenderTiers[currentTier].defenderUnit.defenderName;
            buttonTxtComp.text = "Upgrade to " + nextTier;
            if (currentDefenderTiers[currentTier].costToUpgrade > creditsDisplay.GetCredits()) {
                buyButton.enabled = false;
                buttonTxtComp.color = greyedOutColour;
            } else {
                buyButton.enabled = true;
                buttonTxtComp.color = Color.white;
            }
        }
        
    }

    public void UpgradeCurrentShip() {
        int currentTier = PlayerData.GetShipTier(currentDefender.shipFamily);
        creditsDisplay.SpendCredit(currentDefenderTiers[currentTier].costToUpgrade);
        FindObjectOfType<PlayerData>().UpgradeShip(currentDefender.shipFamily);
        // Update all icons
        foreach (ShopSelector selector in selectors) {
            selector.UpdateIcon();
        }
        // Update shop contents UI
        UpdatePanels();
    }

    private void Update() {
        Debug.Log("Currently selected ship: " + currentDefender);
    }

    public void SetCurrentDefender(Defender ship, Defender[] tiers) {
        currentDefender = ship;
        currentDefenderTiers = tiers;
        UpdatePanels();
    }
}
