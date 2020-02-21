using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTranslate : MonoBehaviour {
    public float backgroundSpeed;

    private void Update() {
        transform.Translate(Vector3.left * backgroundSpeed * Time.deltaTime);
    }
}
