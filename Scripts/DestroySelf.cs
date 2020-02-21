using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Workaround for persistent gameobjects that were meant to be destroyed soon after being instantiated
public class DestroySelf : MonoBehaviour {
    [SerializeField] float lifeDelay;

    void Start() {
        StartCoroutine(DisableAfterDelay());
    }

    IEnumerator DisableAfterDelay() {
        yield return new WaitForSeconds(lifeDelay);
        Destroy(gameObject);
    }
}
