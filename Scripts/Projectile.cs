using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    // ===== Projectile properties =====
    [SerializeField]
    [Tooltip("Positive velocity means the projectile travels to the right, negative means to the left")]
    float projectileVelocity = 1.5f;
    [SerializeField]
    int maxDamage = 15;
    [SerializeField]
    int minDamage = 10;
    [SerializeField]
    bool isFriendlyProjectile = true;

    // ===== Projectile SFX =====
    [SerializeField]
    SoundClip laserFireSFX = null;
    [SerializeField]
    SoundClip successfulHitSFX = null;  // Played on collision with a ship

    // ===== Other =====
    [SerializeField]
    float destroyDelay = 2;  // How long the projectile gameobject persists after it is meant to be destroyed. This is part of a workaround for being able to play sounds after an object is 'destroyed'

    void Start() {
        PlaySFX(laserFireSFX);
    }
    void Update() {
        // Move the projectile rightwards
        transform.Translate(Vector2.right * projectileVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Assumes the object with a collider component also has a health script attached
        if (isFriendlyProjectile) {
            if (collision.gameObject.tag == "Enemy") {
                // A projectile fired by a friendly unit collided with an enemy
                Health enemyHealth = collision.gameObject.GetComponent<Health>();
                int damage = Random.Range(minDamage, maxDamage);
                enemyHealth.ReduceHealth(damage);  // ReduceHealth handles death if the enemy has taken enough damage
                // Play successful hit SFX
                PlaySFX(successfulHitSFX);
                // Destroy the projectile
                DisableProjectile();
            }
        } else if (!isFriendlyProjectile) {
            // A projectile fired by an enemy collided with a friendly unit
            if (collision.gameObject.tag == "Defender") {
                Health defenderHealth = collision.gameObject.GetComponent<Health>();
                int damage = Random.Range(minDamage, maxDamage);
                defenderHealth.ReduceHealth(damage);  // ReduceHealth handles death if the defender has taken enough damage
                // Play successful hit SFX
                PlaySFX(successfulHitSFX);
                // Destroy the projectile
                DisableProjectile();
            }
        }
    }

    private void PlaySFX(SoundClip sfx) {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null) {
            // Debug.Log("Master volume: " + PlayerData.GetGameVolume());
            audioSource.clip = sfx.clip;
            audioSource.volume = sfx.volume * PlayerData.GetGameVolume(); ;
            audioSource.pitch = sfx.pitch;
            audioSource.Play();
        }
    }

    // Switches off the sprite and the collider 
    private void DisableProjectile() {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, destroyDelay);
    }
}
