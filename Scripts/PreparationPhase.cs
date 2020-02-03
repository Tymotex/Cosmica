using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationPhase : MonoBehaviour {
    

    public void StartLevel() {
        FindObjectOfType<LevelStatus>().levelStarted = true;
        // Deselect any highlighted tiles
        DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
        foreach (DefenderTile tile in tiles) {
            if (tile.isHighlighted) {
                tile.RemoveHighlight();
                break;
            }
        }
        Destroy(gameObject);
    }
}
