using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    // ===== Health =====
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int health;
    [SerializeField]
    GameObject healthBarParent = null;  // Has a sprite renderer component containing the health bar's background sprite
    [SerializeField]
    HealthBar healthBar = null;
    bool isDamaged = false;

    // ===== Popups =====
    [SerializeField]
    float popupLife = 1.5f;  // How long the popup persists (allow enough time for the animation)
    [SerializeField]
    Vector3 popupOffset = new Vector3(0, 0, 0);  // Spawn the popup a specific offset from the centre of the tile
    [SerializeField]
    Popup damagePopup = null;

    // ===== On death =====
    [SerializeField]
    int energyGainOnKill = 25;
    [SerializeField]
    [Tooltip("Raw percentage (eg. 2 means 2% control). Set a negative value to lose control when a defender unit is destroyed, for example")]
    float controlGainOnKill = 2f;

    // ===== Effects =====
    [SerializeField]
    [Tooltip("Add a particle system here")]
    GameObject deathVFX = null;

    void Start() {
        DisplayHealthIfDamaged();
    }

    public void DisplayHealthIfDamaged() {
        if (maxHealth == health) {
            isDamaged = false;
        } else {
            isDamaged = true;
        }

        if (isDamaged) {
            healthBarParent.GetComponent<SpriteRenderer>().enabled = true;
            healthBar.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            healthBarParent.GetComponent<SpriteRenderer>().enabled = false;
            healthBar.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ReduceHealth(int damage) {
        health -= damage;
        SpawnNotification(damagePopup, damage);
        DisplayHealthIfDamaged();
        if (healthBar != null) {
            healthBar.UpdateHealthBar(maxHealth, health);
        }
        if (health <= 0) {
            die();
        }
    }

    private void die() {
        LevelStatus levelStatus = FindObjectOfType<LevelStatus>();
        levelStatus.AddEnergy(energyGainOnKill);
        levelStatus.AddControl(controlGainOnKill);

        // Destroy the object that was hit below 0 health and instantiate an explosion particle system
        Destroy(gameObject);
        GameObject deathExplosion = Instantiate(deathVFX, transform.position, Quaternion.identity) as GameObject;
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
        popup.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        popup.transform.position = transform.position + popupOffset;
        Destroy(popup, popupLife);
    }

    public float GetControlGainOnKill() {  // TODO: Use properties?
        return controlGainOnKill;
    }
    // TODO: Currently doesn't work
    /*private void SpawnDamageNotification(Text popupPrefab, int damageDealt) {
        Debug.Log("Spawning damage popup");
        foreach (Transform child in transform) {  // Destroy any popup currently displayed before instantiating a new one
            if (child.tag == "Popup") {  // Popup gameobjects have the "popup" tag
                Destroy(child.gameObject);
            }
        }
        damagePopup.text = damageDealt.ToString();
        Text popup = Instantiate(popupPrefab, transform.position, Quaternion.identity);
        //popup.transform.SetParent(transform);
        //popup.transform.position = transform.position;
        popup.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        popup.transform.position = transform.position;
        popup.transform.SetParent(transform, true);
        Destroy(popup, popupLife);
    }*/

}
