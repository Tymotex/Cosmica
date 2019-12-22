using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    bool spawning = true;

    [SerializeField]
    float minSpawnInterval = 1f;
    [SerializeField]
    float maxSpawnInterval = 3f;

    [SerializeField]
    GameObject enemy;  // Could change GameObject to our class: EnemyAttack

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
        spawnedEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        spawnedEnemy.transform.position = transform.position;
    }

    void Update() {

    }
}
