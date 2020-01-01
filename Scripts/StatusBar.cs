using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour {
    [SerializeField]
    float sizeOfBar = 2.23f;

    private void Start() {
        UpdateStatusBar(100, 50);
    }

    public void UpdateStatusBar(int maxValue, int currValue) {
        float ratio = (float)currValue / maxValue;
        ScaleDown(ratio);
    }
    public void UpdateStatusBar(float maxValue, float currValue) {
        float ratio = currValue / maxValue;
        ScaleDown(ratio);
    }

    // ratio should always be between 0 and 1
    private void ScaleDown(float ratio) {
        transform.localScale = new Vector3(ratio, 1, 1);
        Vector3 newDisplacement = new Vector3(-(1 - ratio) * (sizeOfBar / 2), 0, 0);
        transform.localPosition = newDisplacement;
    }
}
