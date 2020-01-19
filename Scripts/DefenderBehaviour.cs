using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBehaviour : MonoBehaviour {
    // ===== Stats =====
    const int RANK_LIGHT = 0;
    const int RANK_MEDIUM = 1;
    const int RANK_HEAVY = 2;
    const int RANK_ELITE = 3;
    [Range(0, 3)]
    [Tooltip("0 is light, 1 is medium, 2 is heavy, 3 is elite")]
    public int rank;

    // ===== Shooting =====
    [SerializeField]
    GameObject ammo = null;
    bool isShooting = true;  // Initialise to true to start firing immediately after spawning
    [SerializeField]
    float minShootDelay = 3f;
    [SerializeField]
    float maxShootDelay = 4f;
    [SerializeField]
    Vector3 shootingOffset = Vector3.zero;

    // ==== Info =====
    int currRow;  // Unused

    // ===== Other =====
    [SerializeField]
    SoundClip deathSFX = null;
    public bool isDead = false;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int maxImpactDamage = 108;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int minImpactDamage = 92;

    // ===== Link to parent container =====
    [SerializeField] Defender container = null;

    // ===== On Spawn =====
    [Tooltip("Set this value to how long it takes for the ship to swerve into position.")]
    [SerializeField] float spawnShootDelay = 1;

    public void StartShooting() {
        StartCoroutine(Shoot());
    }

    public void StopShooting() {
        StopAllCoroutines();  // Stops ALL coroutines. TODO: For some reason, calling StopCoroutine(Shoot()) doesn't stop the coroutine I started in StartShooting().
        // TODO: Maybe setting isShooting to false solves this
    }

    public IEnumerator Shoot() {
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
        //projectile.transform.SetParent(transform, true);
        projectile.transform.position = transform.position + shootingOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            Health defenderHealth = GetComponent<Health>();
            Health enemyHealth = enemy.GetComponent<Health>();
            Debug.Log("===> Collision with enemy. " + rank + " vs. " + enemy.rank);

            int impactDamageOnEnemy = Random.Range(minImpactDamage, maxImpactDamage);
            int impactDamageOnDefender = Random.Range(enemy.minImpactDamage, enemy.maxImpactDamage);
            defenderHealth.ReduceHealth(impactDamageOnDefender);
            enemyHealth.ReduceHealth(impactDamageOnEnemy);
        }
    }

    public void Die() {
        container.DestroySelf();
    }
}
