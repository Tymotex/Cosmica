using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {
    [SerializeField] float accerlateFactor;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            EnemyBehaviour enemyBehaviour = collision.gameObject.GetComponent<EnemyBehaviour>();
            enemyBehaviour.advanceSpeed *= accerlateFactor;
        }
    }
}
