using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    // ===== Stats =====
    [Range(0f, 5f)]
    public float advanceSpeed = 1f;
    const int RANK_LIGHT = 0;
    const int RANK_MEDIUM = 1;
    const int RANK_HEAVY = 2;
    const int RANK_ELITE = 3;
    [Range(0, 3)]
    [Tooltip("0 is light, 1 is medium, 2 is heavy, 3 is elite")]
    public int rank;

    // ===== Shooting =====
    public bool isShooting = false;  
    [SerializeField] GameObject ammo = null;
    [SerializeField] float minShootDelay = 3f;
    [SerializeField] float maxShootDelay = 4f;
    [SerializeField] Vector3 shootingOffset = Vector3.zero;

    // ===== Other =====
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int maxImpactDamage = 108;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int minImpactDamage = 92;

    // ===== Link to Parent =====
    [SerializeField] Enemy container = null;

    // ===== On Spawn =====
    [Tooltip("Set this value to how long it takes for the ship to swerve into position.")]
    [SerializeField] float spawnShootDelay = 1;

    // ===== On Death =====
    [SerializeField] int chanceToDropCoin = 25;
    [SerializeField] CoinDrop[] coinDrops = null;
    [Tooltip("Make sure this sums to 100 AND is the same size as 'coinDrops'")]
    [SerializeField] int[] coinSpawnChances = null;  

    void Start() {
        isShooting = false;
    }

    public void StartShooting() {
        StartCoroutine(Shoot());
    }

    public void StopShooting() {
        StopAllCoroutines();  // Stops ALL coroutines. TODO: For some reason, calling StopCoroutine(Shoot()) doesn't stop the coroutine I started in StartShooting().
        // TODO: Maybe setting isShooting to false solves this
    }

    void Update() {
        transform.Translate(Vector2.left * advanceSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Random.Range(0, 100) < chanceToDropCoin) {
                Debug.Log("Trying to spawn coin!");
                SpawnCoinDropByChance();
            }
        }
    }

    private IEnumerator Shoot() {
        yield return new WaitForSeconds(spawnShootDelay);
        while (isShooting) {
            SpawnProjectile();
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        }
    }

    private void SpawnProjectile() {
        // Instantiate a projectile as the child of the canvas and set the position at where the defender is
        GameObject projectile = Instantiate(ammo, transform.position, Quaternion.identity);
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        projectile.transform.position = transform.position + shootingOffset;
    }

    public void Die() {
        if (Random.Range(0, 100) < chanceToDropCoin) {
            Debug.Log("Trying to spawn coin!");
            SpawnCoinDropByChance();
        }
        container.DestroySelf();
    }

    public void SpawnCoinDropByChance() {
        int randomNumber = Random.Range(0, 100);  // Random integer in 0, 1, ..., 98, 99
        int lowerBound = 0;
        int higherBound = 99;
        for (int i = 0; i < coinDrops.Length; i++) {
            higherBound = coinSpawnChances[i] + lowerBound;
            if (randomNumber >= lowerBound && randomNumber < higherBound) {  // Successfully rolled coinDrops[i]
                SpawnCoin(coinDrops[i]);
                break;
            }
            lowerBound = higherBound;
        }
    }
     
    public void SpawnCoin(CoinDrop coinPrefab) {
        Debug.Log("Spawned coin!!!!!!!!!!!!!!!!!!!!!!!!!");
        CoinDrop coin = Instantiate(coinPrefab, transform.position, Quaternion.identity) as CoinDrop;
        coin.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        coin.transform.position = new Vector2(transform.position.x, transform.parent.position.y); 
    }
}
