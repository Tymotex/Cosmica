using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawner : MonoBehaviour {
    [SerializeField] float minSpawnInterval = 1;
    [SerializeField] float maxSpawnInterval = 5;
    [SerializeField] float maxVerticalOffset = 2;    // In world units
    // ===== Decoration Objects =====
    [SerializeField] Decoration[] decorations = null;

    // ===== Links =====
    [SerializeField] Canvas gameCanvas = null;

    private void Awake() {
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Canvas>();
    }

    void Start() {
        StartCoroutine(SpawnDecoration());
    }

    private IEnumerator SpawnDecoration() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            int randomIndex = Random.Range(0, decorations.Length);
            Vector3 randomOffset = RandomSpawnPosition();
            Decoration decoration = Instantiate(decorations[randomIndex], Vector3.zero, Quaternion.identity) as Decoration;
            decoration.transform.SetParent(gameCanvas.transform, false);
            decoration.transform.SetParent(transform);
            decoration.transform.position = transform.position + randomOffset;
        }
    }

    // In world units
    Vector3 RandomSpawnPosition() {
        return new Vector3(0, Random.Range(-maxVerticalOffset, maxVerticalOffset), 0);
    }
}
