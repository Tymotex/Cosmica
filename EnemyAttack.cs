using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    [Range(0f, 5f)]
    [SerializeField]
    float advanceSpeed = 1f;

    void Update() {
        transform.Translate(Vector2.left * advanceSpeed * Time.deltaTime);
    }

    // ===== Animation Event Functions =====
    // Invoked by an event marker in an animation

    public void SetSpeed(float speed) {
        advanceSpeed = speed;
    }

    


}
