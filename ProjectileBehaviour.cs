using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

    [SerializeField]
    [Tooltip("Positive value means rightward translation. Negative value means leftward translation")]
    float projectileVelocity;

    void Update() {
        // Move the projectile rightwards
        transform.Translate(Vector3.right * projectileVelocity * Time.deltaTime);
    }
}
