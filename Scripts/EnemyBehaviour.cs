using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    // ===== Stats =====
    [Range(0f, 5f)] public float advanceSpeed = 1f;

    //DEBUG: 
    [SerializeField] float DPS = 0;

    // ===== Shooting =====
    [HideInInspector] public bool enemyIsShooting = false;  
    public Projectile ammo = null;
    public float minShootDelay = 3f;
    public float maxShootDelay = 4f;
    [SerializeField] Vector3 shootingOffset = Vector3.zero;
    [SerializeField] ParticleSystem fireVFX = null;

    // ===== On Collision With Defenders =====
    public int maxImpactDamage = 108;
    public int minImpactDamage = 92;

    // ===== Links =====
    [SerializeField] Enemy container = null;  // Link to parent container
    [SerializeField] Canvas gameCanvas = null; 

    // ===== On Spawn =====
    [Tooltip("Set this value to how long it takes for the ship to swerve into position.")]
    [SerializeField] float spawnShootDelay = 1;

    private void Awake() {
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Canvas>();
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

        // TODO: DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Random.Range(0, 100) < container.chanceToDropCoin) {
                Debug.Log("Trying to spawn coin!");
                container.SpawnCoinDropByChance();
            }
        }
        float avgDamage = (ammo.minDamage + ammo.maxDamage) / 2f;
        float avgFirerate = 1 / ((minShootDelay + maxShootDelay) / 2f);
        DPS = avgDamage * avgFirerate;
    }

    private IEnumerator Shoot() {
        yield return new WaitForSeconds(spawnShootDelay);
        while (enemyIsShooting) {
            SpawnProjectile();
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        }
    }

    private void SpawnProjectile() {
        // Instantiate a projectile as the child of the canvas and set the position at where the defender is
        Projectile projectile = Instantiate(ammo, transform.position, Quaternion.identity) as Projectile;
        projectile.transform.SetParent(gameCanvas.transform, false);
        projectile.transform.position = transform.position + shootingOffset;
        ParticleSystem gunBlast = Instantiate(fireVFX, transform.position, fireVFX.transform.rotation) as ParticleSystem;
        
        gunBlast.transform.position = transform.position + shootingOffset;
        Destroy(gunBlast, 0.2f);
    }

    public void Die(bool killedByPlayer) {
        if (killedByPlayer) {
            if (Random.Range(0, 100) < container.chanceToDropCoin) {
                container.SpawnCoinDropByChance();
            }
        }
        container.DestroySelf();
    }
}
