using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    // ===== Health =====
    public int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] GameObject healthBarParent = null;  // Has a sprite renderer component containing the health bar's background sprite
    [SerializeField] HealthBar healthBar = null;
    bool isDamaged = false;

    // ===== Defence =====
    public int defence;  // Reduces incoming damage by how much defence this unit has

    // ===== Popups =====
    [SerializeField] float popupLife = 1.5f;  // How long the popup persists (allow enough time for the animation)
    [SerializeField] Vector3 popupOffset = new Vector3(0, 0, 0);  // Spawn the popup a specific offset from the centre of the tile
    [SerializeField] Popup damagePopup = null;

    // ===== On death =====
    public int energyGainOnKill = 15;
    [Tooltip("Raw percentage (eg. 2 means 2% control). Set a negative value to lose control when a defender unit is destroyed, for example")]
    public float controlGainOnKill = 2f;
    [Tooltip("Set a negative value to lose score on kill")]
    public int scoreGainOnKill = 20;

    // ===== Effects =====
    [SerializeField] ParticleSystem[] deathVFXs = null;
    [SerializeField] SoundClip deathSFX = null;  // This is played by the projectile

    // ===== Links =====
    [SerializeField] Canvas gameCanvas = null;

    private void Awake() {
        gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Canvas>();
    }

    void Start() {
        DisplayHealthIfDamaged();
    }

    public void DisplayHealthIfDamaged() {
        isDamaged = maxHealth == health ? false : true;
        if (isDamaged) {
            healthBarParent.GetComponent<SpriteRenderer>().enabled = true;
            healthBar.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            healthBarParent.GetComponent<SpriteRenderer>().enabled = false;
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ReduceHealth(int damage) {
        int damageTaken = (damage - defence) >= 0 ? (damage - defence) : 0;
        health -= damageTaken;
        SpawnNotification(damagePopup, damageTaken);
        DisplayHealthIfDamaged();
        if (healthBar != null) {
            healthBar.UpdateHealthBar(maxHealth, health);
        }
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
        levelStatus.AddEnergy(energyGainOnKill);
        levelStatus.AddControl(controlGainOnKill);
        levelStatus.AddScore(scoreGainOnKill);
        // Destroy the object that was hit below 0 health and instantiate an explosion particle system
        if (gameObject.tag == "Enemy") {   
            Projectile.PlayDeathSFX(deathSFX);
            gameObject.GetComponent<EnemyBehaviour>().Die(true);
        } else if (gameObject.tag == "Defender") {
            Projectile.PlayDeathSFX(deathSFX);
            gameObject.GetComponent<DefenderBehaviour>().Die();
        }
        Destroy(gameObject);
        SpawnDeathVFX();
    }

    private void SpawnDeathVFX() {
        int randomIndex = Random.Range(0, deathVFXs.Length);
        ParticleSystem deathExplosion = Instantiate(deathVFXs[randomIndex], transform.position, Quaternion.identity) as ParticleSystem;
        // Need to destroy the particle system after it has completed its full cycle
        float explosionDuration = deathExplosion.GetComponent<ParticleSystem>().main.duration;
        Destroy(deathExplosion, explosionDuration);
    }
    
    private void SpawnNotification(Popup popupPrefab, int damageDealt) {
        foreach (Transform child in transform) {  // Destroy any popup currently displayed before instantiating a new one
            if (child.tag == "Popup") {  // Popup gameobjects have the "popup" tag
                Destroy(child.gameObject);
            }
        }

        Popup popup = Instantiate(popupPrefab, Vector2.zero, Quaternion.identity) as Popup;
        popup.initialText = damageDealt.ToString();
        popup.transform.SetParent(gameCanvas.transform, false);
        popup.transform.position = transform.position + popupOffset;
        Destroy(popup, popupLife);
    }

    public float GetControlGainOnKill() {  // TODO: Use properties?
        return controlGainOnKill;
    }
}
