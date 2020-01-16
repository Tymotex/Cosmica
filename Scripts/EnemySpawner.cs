using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] int rowNumber;  // Used to let any defender units in the same row know that they should be attacking
    public bool spawning = false;
    [SerializeField] float minSpawnInterval = 1f;
    [SerializeField] float maxSpawnInterval = 3f;

    [SerializeField] Enemy[] enemies = null;
    [Tooltip("Must be the same size as the enemies array! Setting the first element of this array to 10% means enemies[0] has 10% chance of being spawned. Make sure the spawn chances at up to 100")]
    public SpawnChances[] chances = null;  // Could be a float...
    [HideInInspector] public int rampIndex = 0;
    LevelStatus levelStatus;

    IEnumerator SpawnEnemies() {
        while (spawning) {
            // Allow time between each enemy spawn
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            // Check whether the difficulty should be ramped up
            // TODO:
            // Select an enemy by spawn percentage chance:
            int randomNumber = Random.Range(0, 100);  // Random integer in 0, 1, ..., 98, 99
            int lowerBound = 0;
            int higherBound = 99;
            print("RAMPINDEX: " + rampIndex);
            for(int i = 0; i < enemies.Length; i++) {
                higherBound = chances[rampIndex].spawnChances[i] + lowerBound;
                if (randomNumber >= lowerBound && randomNumber < higherBound) {
                    // Successfully rolled enemies[i]. Spawning this and breaking out of the loop
                    print("===> Spawning enemy: " + i);
                    SpawnEnemy(enemies[i]);
                    break;
                }
                lowerBound = higherBound;
            }
        }
    }

    void Start() {
        levelStatus = FindObjectOfType<LevelStatus>(); 
    }

    void Update() {
        if (!spawning && levelStatus.levelStarted) {
            spawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    private void SpawnEnemy(Enemy enemy) {
        Enemy spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity) as Enemy;
        spawnedEnemy.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        spawnedEnemy.transform.SetParent(transform);
        spawnedEnemy.transform.position = transform.position;
    }

    // Spawner tile has an enemy unit as its child
    public bool EnemyExistsInRow() {
        foreach(Transform child in transform) {
            if (child.tag == "EnemyContainer") {
                return true;
            }
        }
        return false;
    }

    /*
    public bool DefendersExistInRow() {
        bool defenderPresent = false;
        foreach(DefenderTile tile in defenderTilesInThisRow) {
            foreach(Transform tileChild in tile.transform) {
                if (tileChild.tag == "DefenderBehaviour") {
                    // DefenderBehaviour is present in some tile in the same row as this enemy spawner
                    defenderPresent = true;
                    break;
                }
            }
        }
        return defenderPresent;
    }
    */
}
