using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {
    [SerializeField] string defenderName = "";
    public int currDefenderCost;
    [SerializeField] Defender defenderPrefab = null;
    [SerializeField] GameObject spawnGlowPrefab = null;
    [SerializeField] bool isCurrSelected = false;
    /*
    [SerializeField]
    bool isUnlocked = true;  // Should be greyed out if not yet unlocked
    */
    private void OnMouseDown() {
        Debug.Log("Selector: selected " + defenderName);
        Selector[] allUnits = FindObjectsOfType<Selector>();
        // If there already exists a currently selected unit, then deselect it
        foreach (Selector unit in allUnits) {
            if (unit == this) {
                if (!isCurrSelected) {
                    isCurrSelected = true;
                    spawnGlow();
                    continue;
                } else {
                    isCurrSelected = false;
                    destroyGlow();
                    continue;
                }
            }
            if (unit.isCurrSelected == true) {
                unit.isCurrSelected = false;
                unit.destroyGlow();
            }
            
        }

        DefenderTile[] allTiles = FindObjectsOfType<DefenderTile>();  // TODO: Inefficient
        // Loop through all the tiles in the battlefield and sets the defender to spawn to the newly selected unit
        foreach (DefenderTile tile in allTiles) {
            if (isCurrSelected) {
                // Tells the tile spawner that if the player clicks on a tile, spawn the one they selected
                tile.defenderPrefab = defenderPrefab;
            } else {
                // The player deselected this current gameobject's unit, so tell all tile spawners there is nothing currently selected
                tile.defenderPrefab = null;
            }
        }
    }

    private void OnMouseEnter() {
        
    }

    // Spawns a glowing particle system to indicate which defender was selected
    private void spawnGlow() {
        GameObject glow = Instantiate(spawnGlowPrefab, transform.position, Quaternion.identity) as GameObject;
        glow.transform.SetParent(transform);
        glow.transform.position = transform.position;
        glow.transform.localScale = new Vector2(1f, 1f);
    }

    private void destroyGlow() {
        // Destroys all children, so this assumes we aren't deleting anything other than the glow particles...
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

}
