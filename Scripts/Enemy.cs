using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] EnemyBehaviour enemyUnit = null;


    public void DestroySelf() {
        Destroy(gameObject);
    }
}
