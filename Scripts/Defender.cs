using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour {
    public DefenderBehaviour defenderUnit = null;
    public int costToSpawn;
    [Tooltip("Moving units across tiles costs energy")]
    public int costToMove;
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
        DefenderTile currTile = GetCurrentTile();
        currTile.UpdateValidTileHighlight();
        currTile.RemoveHighlight();
        Destroy(gameObject);
    }

    public void PlayVictoryAnimation() {
        GetComponent<Animator>().SetBool("levelCompleted", true);
    }

    public DefenderTile GetCurrentTile() {
        return transform.parent.GetComponent<DefenderTile>();
    }
}
