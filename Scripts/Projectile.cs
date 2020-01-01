using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    [Tooltip("Positive velocity means the projectile travels to the right, negative means to the left")]
    float projectileVelocity = 1.5f;
    [SerializeField]
    int maxDamage = 15;
    [SerializeField]
    int minDamage = 10;

    [SerializeField]
    bool isFriendlyProjectile = true;

    void Update() {
        // Move the projectile rightwards
        transform.Translate(Vector2.right * projectileVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Assumes the object with a collider component also has a health script attached
        if (isFriendlyProjectile) {
            if (collision.gameObject.tag == "Enemy") {
                Debug.Log("HIT!");
                // A projectile fired by a friendly unit collided with an enemy
                Health enemyHealth = collision.gameObject.GetComponent<Health>();
                int damage = Random.Range(minDamage, maxDamage);
                enemyHealth.ReduceHealth(damage);  // ReduceHealth handles death if the enemy has taken enough damage
                
                Destroy(gameObject);  // Destroy the projectile
            }
        } else {
            // A projectile fired by an enemy collided with a friendly unit
            if (collision.gameObject.tag == "Defender") {
                Health defenderHealth = collision.gameObject.GetComponent<Health>();
                int damage = Random.Range(minDamage, maxDamage);
                defenderHealth.ReduceHealth(damage);  // ReduceHealth handles death if the defender has taken enough damage
                Destroy(gameObject);  // Destroy the projectile
            }
        }
    }
}
