using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour {
    public DefenderBehaviour defenderUnit = null;
    public int costToSpawn;

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void PlayVictoryAnimation() {
        GetComponent<Animator>().SetBool("levelCompleted", true);
    }
}
