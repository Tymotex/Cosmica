using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSelector : MonoBehaviour {
    [SerializeField] ShopManager manager = null;
    public Defender[] tiers;
    public Sprite[] tierSprites;
    int currentTier;
    string shipFamily;
    [SerializeField] Text shipName;
    [SerializeField] Color32 lockedColour;
    [SerializeField] int zoneForUnlock;
    [SerializeField] int levelForUnlock;

    void Start() {
        if (UnitIsUnlocked()) {
            shipFamily = tiers[0].shipFamily;
            UpdateIcon();
        } else {
            GetComponent<Image>().color = lockedColour;
        }
    }

    public void UpdateIcon() {
        currentTier = PlayerData.GetShipTier(shipFamily);
        GetComponent<Image>().sprite = tierSprites[currentTier - 1];
        shipName.text = tiers[currentTier - 1].defenderUnit.defenderName;
    }

    public void SetAsCurrentDefender() {
        if (UnitIsUnlocked()) {
            currentTier = PlayerData.GetShipTier(shipFamily);
            manager.SetCurrentDefender(tiers[currentTier - 1], tiers);
        }
    }

    private bool UnitIsUnlocked() {
        if (PlayerData.LevelIsUnlocked(zoneForUnlock, levelForUnlock)) {
            return true;
        }
        return false;
    }
}
