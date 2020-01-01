using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // ===== Stats =====
    [Range(0f, 5f)]
    [SerializeField]
    float advanceSpeed = 1f;

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

    // ===== =====

    float swerveTime;

    void Start() {
        GetComponent<Animator>().SetBool("swerveComplete", false);
        swerveTime = GetComponent<Animation>().GetClip("Enemy_Swerve").length;
        StartCoroutine(Shoot());
        StartCoroutine(Swerve());
        Debug.Log("swerveTime: " + swerveTime);
    }

    private IEnumerator Swerve() {
        yield return new WaitForSeconds(swerveTime);
        GetComponent<Animator>().SetBool("swerveComplete", true);
    }

    private IEnumerator Shoot() {
        while (isShooting) {
            yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {
        // Instantiate a projectile as the child of the canvas and set the position at where the defender is
        GameObject projectile = Instantiate(ammo, transform.position, Quaternion.identity);
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        projectile.transform.position = transform.position + shootingOffset;
    }

    void Update() {
        if (GetComponent<Animator>().GetBool("swerveComplete")) {
            Debug.Log("Moving");
            transform.Translate(Vector2.left * advanceSpeed * Time.deltaTime);
        }
    }

    // ===== Animation Event Functions =====
    // Invoked by an event marker in an animation

    public void SetSpeed(float speed) {
        advanceSpeed = speed;
    }
}
