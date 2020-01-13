using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // ===== Stats =====
    [Range(0f, 5f)]
    [SerializeField]
    float advanceSpeed = 1f;
    const int RANK_LIGHT = 0;
    const int RANK_MEDIUM = 1;
    const int RANK_HEAVY = 2;
    const int RANK_ELITE = 3;
    [Range(0, 3)]
    [Tooltip("0 is light, 1 is medium, 2 is heavy, 3 is elite")]
    public int rank;

    // ===== Shooting =====
    bool isShooting = true;  // Initialise to true to start firing immediately after spawning. TODO: Is this necessary?
    [SerializeField]
    GameObject ammo = null;
    [SerializeField]
    float minShootDelay = 3f;
    [SerializeField]
    float maxShootDelay = 4f;
    [SerializeField]
    Vector3 shootingOffset = Vector3.zero;

    // ===== Other =====
    [SerializeField]
    SoundClip deathSFX = null;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int maxImpactDamage = 108;
    [Tooltip("~100 for light, ~260 for medium, ~420 for heavy, ~1050 for elite")]
    public int minImpactDamage = 92;

    float swerveTime;

    void Start() {
        GetComponent<Animator>().SetBool("swerveComplete", false);
        swerveTime = GetComponent<Animation>().GetClip("Enemy_Swerve").length;
        StartCoroutine(Shoot());
        StartCoroutine(Swerve());
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
            transform.Translate(Vector2.left * advanceSpeed * Time.deltaTime);
        }
    }
    

    public void PlayDeathSFX() {
        AudioSource.PlayClipAtPoint(deathSFX.clip, FindObjectOfType<Camera>().transform.position, deathSFX.volume);
    }

    // ===== Animation Event Functions =====
    // Invoked by an event marker in an animation

    public void SetSpeed(float speed) {
        advanceSpeed = speed;
    }
}
