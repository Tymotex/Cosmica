using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour {
    [SerializeField] int zone;
    [SerializeField] int level;
    [SerializeField] Color32 disabledColour;

    void Start() {
        if (!PlayerData.LevelIsUnlocked(zone, level)) {
            GetComponent<Image>().color = disabledColour;
            GetComponent<Button>().enabled = false;
        }
    }
}
