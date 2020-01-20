using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Decoration : MonoBehaviour {
    public float maxSpeed;
    public float minSpeed;
    public float minScaleFactor;
    public float maxScaleFactor;
    float speed;


    void Start() {
        speed = Random.Range(minSpeed, maxSpeed);
        SetRandomScale();
    }

    void Update() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void SetRandomScale() {
        float scaleFactor = Random.Range(minScaleFactor, maxScaleFactor);
        transform.localScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y * scaleFactor, transform.localScale.z * scaleFactor);
    }
}
