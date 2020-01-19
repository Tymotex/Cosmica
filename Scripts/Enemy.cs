using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public EnemyBehaviour enemyUnit = null;


    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void PlayDefeatAnimation() {
        GetComponent<Animator>().SetBool("levelCompleted", true);
        enemyUnit.transform.rotation = new Quaternion(0, 180, 0, 0);
    }
}
