using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyBehaviour enemyUnit = null;
    [SerializeField] SoundClip spawnSFX = null;
    AudioSource audioSource;

    // ===== On Death =====
    public int chanceToDropCoin = 25;
    [SerializeField] CoinDrop[] coinDrops = null;
    [Tooltip("Make sure this sums to 100 AND is the same size as 'coinDrops'")]
    [SerializeField] int[] coinSpawnChances = null;

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

    public void SpawnCoinDropByChance() {
        int randomNumber = Random.Range(0, 100);  // Random integer in 0, 1, ..., 98, 99
        int lowerBound = 0;
        int higherBound = 99;
        for (int i = 0; i < coinDrops.Length; i++) {
            higherBound = coinSpawnChances[i] + lowerBound;
            if (randomNumber >= lowerBound && randomNumber < higherBound) {  // Successfully rolled coinDrops[i]
                SpawnCoin(coinDrops[i]);
                break;
            }
            lowerBound = higherBound;
        }
    }

    public void SpawnCoin(CoinDrop coinPrefab) {
        CoinDrop coin = Instantiate(coinPrefab, transform.position, Quaternion.identity) as CoinDrop;
        coin.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        coin.transform.position = new Vector2(transform.position.x, transform.parent.position.y);
    }
}
