using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [Tooltip("Manually drag and drop the enemy into this field.")]
    public int enemyIndex;
    public Enemy enemy;
    [Tooltip("Uncheck this if the unit doesn't spawn this level.")]
    public bool spawnsThisLevel;

    [SerializeField] GameObject tile = null;
    public Color32 greyedOutColour;

    [SerializeField] EnemyInfoPanel infoPanelPrefab = null;
    [SerializeField] float infoSpawnDelay = 0.25f;
    [SerializeField] Vector3 infoPanelOffset;

    // ===== Links =====
    [SerializeField] Canvas infoPanelCanvas = null;

    private void Awake() {
        infoPanelCanvas = GameObject.FindGameObjectWithTag("InfoPanelCanvas").GetComponent<Canvas>();
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (enemyIndex < spawner.enemies.Length) {
            enemy = spawner.enemies[enemyIndex];
        } else {
            spawnsThisLevel = false;
        }
    }

    void Start() {
        if (enemy != null) {
            this.GetComponent<SpriteRenderer>().sprite = enemy.enemyUnit.GetComponent<SpriteRenderer>().sprite;
        }

        if (spawnsThisLevel) {

        } else {
            tile.GetComponent<SpriteRenderer>().color = greyedOutColour;
            this.GetComponent<SpriteRenderer>().color = greyedOutColour;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (spawnsThisLevel) {
            StartCoroutine(SpawnInfoPanel());
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        StopAllCoroutines();
        Destroy(GameObject.FindGameObjectWithTag("EnemyInfoPanel"));
    }

    private IEnumerator SpawnInfoPanel() {
        if (infoPanelPrefab != null) {
            yield return new WaitForSeconds(infoSpawnDelay);
            EnemyInfoPanel infoPanel = Instantiate(infoPanelPrefab, transform.position, Quaternion.identity) as EnemyInfoPanel;
            infoPanel.transform.SetParent(infoPanelCanvas.transform, false);
            infoPanel.transform.position = transform.position + infoPanelOffset;
            infoPanel.SetCurrentUnit(enemy);
        }
    }
}
