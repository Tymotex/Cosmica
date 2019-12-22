using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderAttack : MonoBehaviour {
    [SerializeField]
    GameObject ammo;

    bool isShooting = true;

    [SerializeField]
    float minShootDelay = 3f;
    [SerializeField]
    float maxShootDelay = 4f;

    void Start() {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {
        while (isShooting) {
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {
        // Instantiate a projectile as the child of the canvas and set the position at where the defender is
        GameObject projectile = Instantiate(ammo, transform.position, Quaternion.identity);
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        projectile.transform.position = transform.position;
    }

    void Update() {
        
    }
}
