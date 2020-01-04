using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour {
    // ===== Stats =====
    public int costToSpawn;

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

    public void StartShooting() {
        StartCoroutine(Shoot());
    }

    public void StopShooting() {
        StopAllCoroutines();  // Stops ALL coroutines. TODO: For some reason, calling StopCoroutine(Shoot()) doesn't stop the coroutine I started in StartShooting().
        // TODO: Maybe setting isShooting to false solves this
    }

    public IEnumerator Shoot() {
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

    public void PlayDeathSFX() {
        AudioSource.PlayClipAtPoint(deathSFX.clip, FindObjectOfType<Camera>().transform.position, 3);
    }
}
