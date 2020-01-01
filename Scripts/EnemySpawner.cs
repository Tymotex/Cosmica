using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    int rowNumber;  // Used to let any defender units in the same row know that they should be attacking

    bool spawning = true;

    [SerializeField]
    float minSpawnInterval = 1f;
    [SerializeField]
    float maxSpawnInterval = 3f;

    [SerializeField]
    GameObject enemy = null;  // Could change GameObject to our class: EnemyAttack

    IEnumerator Start() {
        while (spawning) {
            // Allow time between each enemy spawn
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            // Spawn a new enemy
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        GameObject spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity) as GameObject;
        // spawnedEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        spawnedEnemy.transform.parent = transform;
        spawnedEnemy.transform.position = transform.position;
    }

    // Spawner tile has an enemy unit as its child
    public bool EnemyExistsInRow() {
        foreach(Transform child in transform) {
            if (child.tag == "Enemy") {
                return true;
            }
        }
        return false;
    }
}
