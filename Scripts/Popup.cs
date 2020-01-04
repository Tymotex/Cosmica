using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {
    [SerializeField]
    Text popup = null;
    [SerializeField]
    float popupLife = 1.5f;  // TODO: The "no defenders" and "insufficient energy" popups do not have this script attached

    public string initialText = "Blank";

    void Start() {
        if (popup != null) {
            popup.text = initialText;
            Destroy(gameObject, popupLife);
        }
    }
}
