using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderTile : MonoBehaviour {
    public Defender defenderPrefab;

    [SerializeField]
    int rowNumber = 1;
    public EnemySpawner targetSpawner = null;  // The enemy spawner on the same row as the tile

    [SerializeField] Defender defenderOnTile = null;  // Reference to the defender that's on this tile

    // ===== Popup =====
    [SerializeField]
    float popupLife = 1.5f;  // How long the popup displays for
    [SerializeField]
    Vector3 popupOffset = new Vector3(0, 0, 0);  // Spawn the popup a specific offset from the centre of the tile
    [SerializeField]
    GameObject insuffEnergyPopup = null;  // Text UI element that pops up when the player attempts to spend more energy than they have
    [SerializeField]
    GameObject noDefenderPopup = null;  // Text UI element that pops up when the player attempts to spend more energy than they have

    // ===== Highlighting Units =====
    [SerializeField] ParticleSystem selectionGlow = null;
    public static bool highlighted = false;   // Keeps track of whether there is currently a unit highlighted
    public bool isHighlighted = false;        // Keeps track of whether THIS INSTANCE is the one that's highlighted
    [SerializeField] Color32 validTileColour;
    [SerializeField] ParticleSystem validTileGlow = null;

    // ===== Link =====
    [SerializeField] LevelStatus levelStatus = null;

    void Update() {
        if (defenderOnTile != null) {
            if (targetSpawner.EnemyExistsInRow() == true && defenderOnTile.defenderUnit.isShooting == false) {
                defenderOnTile.defenderUnit.StartShooting();
                defenderOnTile.defenderUnit.isShooting = true;
            } else if (targetSpawner.EnemyExistsInRow() == false && defenderOnTile.defenderUnit.isShooting == true) {
                defenderOnTile.defenderUnit.StopShooting();
                defenderOnTile.defenderUnit.isShooting = false;
            }
        }
    }

    // OnMouseDown is called whenever the mouse clicks on a region within the tile's collider region (the circle region)
    public void OnMouseDown() {
        if (!levelStatus.levelStarted) {
            if (highlighted) {
                if (DefenderIsPresent()) {
                    if (isHighlighted) {
                        Debug.Log("Removing highlight since same unit was clicked again");
                        RemoveHighlight();
                    } else {
                        // Deselect any highlighted tiles
                        DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
                        foreach (DefenderTile tile in tiles) {
                            if (tile.isHighlighted) {
                                tile.RemoveHighlight();
                                break;
                            }
                        }
                        ToggleHighlight();
                    }
                } else {
                    MoveUnitHere();
                }
            } else if (!highlighted) {
                if (!DefenderIsPresent()) {
                    if (defenderPrefab != null) {
                        SpawnDefender();
                    } else {
                        Debug.Log("Not highlighted and no defender selected");
                        SpawnNotification(noDefenderPopup);
                    }
                } else if (DefenderIsPresent()) {
                    ToggleHighlight();
                }
            }
        } else {
            if (!DefenderIsPresent()) {
                if (defenderPrefab != null) {
                    SpawnDefender();
                } else {
                    SpawnNotification(noDefenderPopup);
                }
            }
        }
    }

    // OnMouseOver is called whenever the mouse hovers over the tile
    private void OnMouseOver() {
        if (!DefenderIsPresent()) {
            Animation pulse = GetComponent<Animation>();
            pulse.Play();
        }
    }

    private void SpawnDefender() {
        LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
        // Only spawn a unit if we have enough energy available
        if (levelStatus.energy >= defenderPrefab.costToSpawn) {
            Defender spawnedDefender = Instantiate(defenderPrefab, transform.position, Quaternion.identity) as Defender;
            spawnedDefender.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            spawnedDefender.transform.SetParent(transform);
            spawnedDefender.transform.position = transform.position;
            spawnedDefender.transform.localScale = new Vector3(1, 1, 1);
            levelStatus.SpendEnergy(defenderPrefab.costToSpawn);
            defenderOnTile = spawnedDefender;
        } else {
            SpawnNotification(insuffEnergyPopup);
        }
    }

    private void SpawnNotification(GameObject popupPrefab) {
        foreach (Transform child in transform) {  // Destroy any popup currently displayed before instantiating a new one
            if (child.tag == "Popup") {  // Popup gameobjects have the "popup" tag
                Destroy(child.gameObject);
            }
        }
        GameObject popup = Instantiate(popupPrefab, Vector2.zero, Quaternion.identity) as GameObject;
        popup.transform.SetParent(transform);
        popup.transform.position = transform.position + popupOffset;
        Destroy(popup, popupLife);
    }

    public bool DefenderIsPresent() {
        foreach (Transform child in transform) {
            if (child.tag == "DefenderContainer") {
                return true;
            }
        }
        return false;
    }

    
    public int GetRowNumber() {
        return rowNumber;
    }

    public void TellDefenderToShoot() {
        if (defenderOnTile != null) {
            StartCoroutine(defenderOnTile.defenderUnit.Shoot());
        }
    }

    public void TellDefenderToStopShooting() {
        if (defenderOnTile != null) {
            StopCoroutine(defenderOnTile.defenderUnit.Shoot());
        }
    }

    public void ToggleHighlight() {
        if (highlighted) {
            if (isHighlighted) {  // If the current instance is selected, then deselect it on the second click
                isHighlighted = false;
                highlighted = false;
                Debug.Log("Destroying child");
                foreach (Transform child in transform) {
                    if (child.tag == "Tile Selection Glow") {
                        Destroy(child.gameObject);
                    }
                }
            }
        } else if (!highlighted && !isHighlighted) {
            SpawnGlow(selectionGlow);
            highlighted = true;
            isHighlighted = true;
            // Spawn a glow particle system for all valid tiles in the battlefield
            ToggleValidTileHighlight();
        }
    }

    public void RemoveHighlight() {
        isHighlighted = false;
        highlighted = false;
        foreach (Transform child in transform) {
            if (child.tag == "Tile Selection Glow") {
                Destroy(child.gameObject);
            }
        }
        DestroyAllValidTileGlow();
    }

    public void SpawnGlow(ParticleSystem glowPrefab) {
        ParticleSystem glow = Instantiate(glowPrefab, transform.position, Quaternion.identity) as ParticleSystem;
        glow.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        glow.transform.SetParent(transform);
        glow.transform.position = transform.position;
    }

    public void DestroyAllValidTileGlow() {
        DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
        foreach (DefenderTile tile in tiles) {
            foreach (Transform child in tile.transform) {
                if (child.tag == "Valid Tile Glow") {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    private void ToggleValidTileHighlight() {
        if (highlighted) {
            // Spawn a glow particle system for all valid tiles in the battlefield
            DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
            foreach (DefenderTile tile in tiles) {
                if (!tile.DefenderIsPresent()) {
                    tile.SpawnGlow(validTileGlow);
                }
            }
        } else {
            DestroyAllValidTileGlow();
        }
    }

    public void UpdateValidTileHighlight() {
        if (highlighted) {
            SpawnGlow(validTileGlow);
        }
    }

    private void MoveUnitHere() {
        DefenderTile[] tiles = FindObjectsOfType<DefenderTile>();
        Defender unitToMove = null;
        // Search for the unitToMove
        foreach (DefenderTile tile in tiles) {
            if (tile.isHighlighted == true) {
                tile.RemoveHighlight();
                foreach (Transform child in tile.transform) {
                    if (child.tag == "DefenderContainer") {
                        unitToMove = child.GetComponent<Defender>();
                    }
                }
                // break;
            }
        }
        // Check and deduct energy
        LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
        if (levelStatus.levelStarted) {  // Only charge the player energy if they are out of preparation phase
            if (levelStatus.energy < unitToMove.costToMove) {  // Insufficient energy
                SpawnNotification(insuffEnergyPopup);
                return;
            } else {
                levelStatus.SpendEnergy(unitToMove.costToMove);
            }
        }
        // Move the unit to this tile
        unitToMove.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        unitToMove.transform.SetParent(transform);
        unitToMove.transform.position = transform.position;
        unitToMove.transform.localScale = new Vector3(1, 1, 1);
        defenderOnTile = unitToMove;
    }
}
