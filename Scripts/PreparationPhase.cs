using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationPhase : MonoBehaviour {
    public void StartLevel() {
        FindObjectOfType<LevelStatus>().levelStarted = true;
        Destroy(gameObject);
    }
}
