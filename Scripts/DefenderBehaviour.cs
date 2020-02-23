using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBehaviour : MonoBehaviour {
    // ===== Stats =====
    public string defenderName;
    public int costToSpawn;

    // ===== Shooting =====
    public Projectile ammo = null;
    [HideInInspector] public bool defenderIsShooting = false;  
    public float minShootDelay = 3f;
    public float maxShootDelay = 4f;
    [SerializeField] Vector3 shootingOffset = Vector3.zero;

    // ===== Other =====
    [SerializeField] SoundClip deathSFX = null;
    public bool isDead = false;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int maxImpactDamage = 108;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int minImpactDamage = 92;

    // ===== Links =====
    [SerializeField] Defender container = null;  // This is the parent container
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

    public IEnumerator Shoot() {
        yield return new WaitForSeconds(spawnShootDelay);
        while (defenderIsShooting) {
            SpawnProjectile();
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
        }
    }

    private void SpawnProjectile() {
        // Instantiate a projectile as the child of the canvas and set the position at where the defender is
        Projectile projectile = Instantiate(ammo, transform.position, Quaternion.identity) as Projectile;
        projectile.transform.SetParent(gameCanvas.transform, false);
        //projectile.transform.SetParent(transform, true);
        projectile.transform.position = transform.position + shootingOffset;
        ParticleSystem gunBlast = Instantiate(container.gunFireVFX, transform.position, container.gunFireVFX.transform.rotation) as ParticleSystem;
        gunBlast.transform.position = transform.position + container.gunFireOffset;
        Destroy(gunBlast, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Annihilate the defender ship on collision with enemy (dealing massive damage to the enemy)
        if (collision.gameObject.tag == "Enemy") {
            EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour>();
            if (!enemy.isImmune) {
                Health defenderHealth = GetComponent<Health>();
                Health enemyHealth = enemy.GetComponent<Health>();
                int impactDamageOnEnemy = Random.Range(minImpactDamage, maxImpactDamage);
                int impactDamageOnDefender = Random.Range(enemy.minImpactDamage, enemy.maxImpactDamage);
                defenderHealth.ReduceHealth(impactDamageOnDefender);
                enemyHealth.ReduceHealth(impactDamageOnEnemy, true);
            }
        }
    }

    public void Die() {
        container.DestroySelf();
    }

    // When clicked, it calls OnMouseDown in the DefenderTile which this gameobject is a grandchild of
    // Workaround to a problem where not all the tile's region registers clicks (because this gameobject's collider is blocking them)
    private void OnMouseDown() {
        DefenderTile currTile = container.GetCurrentTile();
        currTile.OnMouseDown();
    }
}
