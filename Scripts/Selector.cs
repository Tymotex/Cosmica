using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // ===== Prefabs and Sprites =====
    public Defender[] tiers;
    public Sprite[] tierSprites;
    int currentTier = 1;
    public Defender defenderPrefab = null;
    [SerializeField] GameObject spawnGlowPrefab = null;
    [SerializeField] InfoPanel infoPanelPrefab = null;

    // ===== Variables =====
    [SerializeField] bool isCurrSelected = false;
    [SerializeField] float infoSpawnDelay = 0.5f;
    [SerializeField] Vector3 infoPanelOffset;
    
    // ===== Unlocking =====
    [Tooltip("zoneForUnlock and levelForUnlock lets the player unlock this unit once they GET UP TO the zone and level set here")]
    public int zoneForUnlock;        
    public int levelForUnlock;
    [SerializeField] Color32 lockedColour;
    bool isUnlocked = true;  // Should be greyed out if not yet unlocked

    void Start() {
        if (!UnitIsUnlocked()) {
            // Debug.Log(defenderPrefab + " is currently locked");
            isUnlocked = false;
            GetComponent<SpriteRenderer>().color = lockedColour;
            GetComponent<CircleCollider2D>().enabled = false;
        } else {
            currentTier = PlayerData.GetShipTier(defenderPrefab.shipFamily);
            defenderPrefab = tiers[currentTier - 1];
            GetComponent<SpriteRenderer>().sprite = tierSprites[currentTier - 1];
        }
    }

    private bool UnitIsUnlocked() {
        if (PlayerData.LevelIsUnlocked(zoneForUnlock, levelForUnlock)) {
            return true;
        }
        return false;
    }

    private void OnMouseDown() {
        // Debug.Log("Selector: selected " + defenderName);
        Selector[] allUnits = FindObjectsOfType<Selector>();
        // If there already exists a currently selected unit, then deselect it
        foreach (Selector unit in allUnits) {
            if (unit == this) {
                if (!isCurrSelected) {
                    isCurrSelected = true;
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.volume = PlayerData.GetGameVolume();
                    audioSource.Play();
                    spawnGlow();
                    continue;
                } else {
                    isCurrSelected = false;
                    destroyGlow();
                    continue;
                }
            } else if (unit.isCurrSelected == true) {
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

    public void OnPointerEnter(PointerEventData eventData) {
        if (isUnlocked) {
            StartCoroutine(SpawnInfoPanel());
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopAllCoroutines();
        Destroy(GameObject.FindGameObjectWithTag("InfoPanel"));
    }

    private IEnumerator SpawnInfoPanel() {
        if (infoPanelPrefab != null) {
            yield return new WaitForSeconds(infoSpawnDelay);
            InfoPanel infoPanel = Instantiate(infoPanelPrefab, transform.position, Quaternion.identity) as InfoPanel;
            infoPanel.transform.SetParent(GameObject.FindGameObjectWithTag("InfoPanelCanvas").transform, false);
            infoPanel.transform.position = transform.position + infoPanelOffset;
            infoPanel.SetCurrentUnit(this.GetComponent<Selector>());
        }
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
