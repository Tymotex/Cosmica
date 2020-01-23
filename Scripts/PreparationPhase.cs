using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationPhase : MonoBehaviour {
    [SerializeField] float destroyDelay = 2;
    [SerializeField] Image panel = null; 
    [SerializeField] Text prep = null;
    [SerializeField] Button readyButton = null;
    [SerializeField] Text ready = null;
    public void StartLevel() {
        FindObjectOfType<LevelStatus>().levelStarted = true;
        DisableVisuals();
        // Deselect any highlighted tiles
        DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
        foreach (DefenderTile tile in tiles) {
            if (tile.isHighlighted) {
                tile.RemoveHighlight();
                break;
            }
        }
        Destroy(gameObject, destroyDelay);
    }

    private void DisableVisuals() {
        panel.GetComponent<Image>().enabled = false;
        prep.GetComponent<Text>().enabled = false;
        readyButton.GetComponent<Image>().enabled = false;
        ready.GetComponent<Text>().enabled = false;
    }
}
