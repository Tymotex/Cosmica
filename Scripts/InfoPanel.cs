using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
    [HideInInspector] public Selector currentUnit;
    [SerializeField] Text infoText = null;

    void Display() {
        if (currentUnit != null) {
            DefenderBehaviour unit = currentUnit.defenderPrefab.defenderUnit;
            Debug.Log("HERE with unit " + unit + " NAME: " + unit.name);

            float avgDamage = (unit.ammo.minDamage + unit.ammo.maxDamage) / 2;
            float avgFirerate = 1 / ((unit.minShootDelay + unit.maxShootDelay) / 2);
            float avgDPS = avgDamage * avgFirerate;
            infoText.text = unit.defenderName +
                "\n" + unit.costToSpawn +
                "\n" + avgDamage.ToString("0.##") +
                "\n" + avgFirerate.ToString("0.##") + "/sec" +
                "\n" + avgDPS.ToString("0.##") +
                "\n" + unit.GetComponent<Health>().maxHealth;
        } else {
            Debug.Log("CURRENT UNIT IS NULL");
        }
    }

    public void SetCurrentUnit(Selector selector) {
        currentUnit = selector;
        Display();
    }
}
