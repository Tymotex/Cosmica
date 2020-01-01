using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderTile : MonoBehaviour {
    public Defender defenderPrefab;

    [SerializeField]
    int rowNumber = 1;
    [SerializeField]
    EnemySpawner targetSpawner = null;  // The enemy spawner on the same row as the tile

    [SerializeField]
    Defender defenderOnTile = null;  // Reference to the defender that's on this tile

    [SerializeField]
    bool isShooting = false;

    // ===== Popup =====
    [SerializeField]
    float popupLife = 1.5f;  // How long the popup displays for
    [SerializeField]
    Vector3 popupOffset = new Vector3(0, 0, 0);  // Spawn the popup a specific offset from the centre of the tile
    [SerializeField]
    GameObject insuffEnergyPopup = null;  // Text UI element that pops up when the player attempts to spend more energy than they have
    [SerializeField]
    GameObject noDefenderPopup = null;  // Text UI element that pops up when the player attempts to spend more energy than they have

    private void Update() {
        if (defenderOnTile != null) {
            if (targetSpawner.EnemyExistsInRow() && isShooting == false) {
                defenderOnTile.StartShooting();
                isShooting = true;
            } else if (targetSpawner.EnemyExistsInRow() == false) {
                defenderOnTile.StopShooting();
                isShooting = false;
            }
        }
    }

    // OnMouseDown is called whenever the mouse clicks on a region within the tile's collider region (the circle region)
    private void OnMouseDown() {
        if (defenderPrefab != null) {
            SpawnDefender();
        } else {
            SpawnNotification(noDefenderPopup);
        }
    }

    // OnMouseOver is called whenever the mouse hovers over the tile
    private void OnMouseOver() {
        if (!DefenderIsPresent()) {
            Animation pulse = GetComponent<Animation>();
            pulse.Play();
        }
    }

    // OnMouseExit is called when the mouse is no longer hovering over the tile
    private void OnMouseExit() {
        
    }

    private void SpawnDefender() {
        if (DefenderIsPresent()) {
            // Not allowed to spawn more than one defender on a single tile
            Debug.Log("DefenderTile: Defender already exists on the tile!");
        } else {
            LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
            // Only spawn a unit if we have enough energy available
            if (levelStatus.energy >= defenderPrefab.costToSpawn) {
                Defender spawnedDefender = Instantiate(defenderPrefab, transform.position, Quaternion.identity) as Defender;
                spawnedDefender.transform.parent = transform;
                spawnedDefender.transform.position = transform.position;
                levelStatus.SpendEnergy(defenderPrefab.costToSpawn);

                defenderOnTile = spawnedDefender;
            } else {
                SpawnNotification(insuffEnergyPopup);
            }
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
            if (child.tag == "Defender") {
                return true;
            }
        }
        return false;
    }

    
    public int GetRowNumber() {
        return rowNumber;
    }
    /*
    // Maybe for a command to shift all units to different tiles
    public void SetRowNumber(int newRowNumber) {
        rowNumber = newRowNumber;
    }
    */

    public void TellDefenderToShoot() {
        if (defenderOnTile != null) {
            StartCoroutine(defenderOnTile.Shoot());
        }
    }

    public void TellDefenderToStopShooting() {
        if (defenderOnTile != null) {
            StopCoroutine(defenderOnTile.Shoot());
        }
    }
}
