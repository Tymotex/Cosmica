using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {
    // The left shredder handles the loss of control when enemies reach beyond the left side
    // Right shredder just destroys projectiles
    // Left shredder destroys projectiles and enemy units that reach it

    [SerializeField]
    bool isLeftShredder = false;
    LevelStatus levelStatus;
    public int controlLossOnCross;  // Fix a penalty rather than use the value from the enemy

    private void Start() {
        levelStatus = FindObjectOfType<LevelStatus>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isLeftShredder) {
            if (collision.gameObject.tag == "Enemy") {
                LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
                Health enemyStats = collision.gameObject.GetComponent<Health>();
                levelStatus.AddControl(-(enemyStats.GetControlGainOnKill()));  // Lose control because the enemy has crossed the border
                                                                               // TODO: Maybe make the control lost a separate variable that we can tune.
                collision.gameObject.GetComponent<EnemyBehaviour>().Die(false);
                Destroy(collision.gameObject);
            } else if (collision.gameObject.tag == "Projectile") {
                Destroy(collision.gameObject);
            } else {
                if (collision.gameObject.tag == "Defender") {

                } else {
                    Destroy(collision.gameObject);
                }
            }
        } else if (!isLeftShredder) {
            if (collision.gameObject.tag == "Projectile") {
                Destroy(collision.gameObject);
            }
        }
    }
}
