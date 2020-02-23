using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationPhase : MonoBehaviour {
    

    public void StartLevel() {
        LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
        levelStatus.levelStarted = true;
        levelStatus.StartRegeneratingEnergy(); 
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
