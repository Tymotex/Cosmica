using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField]
    GameObject barBackground = null;

    public void UpdateHealthBar(int maxHealth, int currHealth) {
        float healthRatio = (float)currHealth / maxHealth;
        ScaleDown(healthRatio);
    }

    // healthRatio should always be between 0 and 1
    private void ScaleDown(float healthRatio) {
        transform.localScale = new Vector3(healthRatio, 1, 1);
    }
}
