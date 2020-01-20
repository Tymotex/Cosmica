using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyBehaviour enemyUnit = null;
    [SerializeField] SoundClip spawnSFX = null;
    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = spawnSFX.clip;
        audioSource.volume = spawnSFX.volume * PlayerData.GetGameVolume();
        audioSource.pitch = spawnSFX.pitch;
        audioSource.Play();
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void PlayDefeatAnimation() {
        GetComponent<Animator>().SetBool("levelCompleted", true);
        enemyUnit.transform.rotation = new Quaternion(0, 180, 0, 0);
    }
}
