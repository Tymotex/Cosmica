using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour {
    public DefenderBehaviour defenderUnit;
    public string shipFamily;
    [SerializeField] SoundClip spawnSFX = null;
    AudioSource audioSource;
    [Tooltip("This is the cost for upgrading the previous tier to this tier. Not applicable to the lowest tier")]
    public int costToUpgrade;  

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
