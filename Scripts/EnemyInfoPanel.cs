using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPanel : MonoBehaviour {
    public Enemy currentEnemy;
    [SerializeField] Text infoText = null;

    void Display() {
        if (currentEnemy != null) {
            EnemyBehaviour unit = currentEnemy.enemyUnit;
            float avgDamage = (unit.ammo.minDamage + unit.ammo.maxDamage) / 2f;
            float avgFirerate = 1 / ((unit.minShootDelay + unit.maxShootDelay) / 2f);
            float avgDPS = avgDamage * avgFirerate;
            infoText.text = currentEnemy.enemyName +
                "\n" + unit.GetComponent<Health>().energyGainOnKill +
                "\n" + unit.GetComponent<Health>().scoreGainOnKill + 
                "\n" + unit.GetComponent<Health>().controlGainOnKill + "%" +  
                "\n" + avgDamage.ToString("0.##") +
                "\n" + avgFirerate.ToString("0.##") + "/sec" +
                "\n" + avgDPS.ToString("0.##") +
                "\n" + unit.GetComponent<Health>().maxHealth.ToString() +
                "\n" + unit.GetComponent<Health>().defence.ToString();
        } else {
            Debug.LogWarning("CURRENT UNIT IS NULL");
        }
    }

    public void SetCurrentUnit(Enemy highlightedEnemy) {
        currentEnemy = highlightedEnemy;
        Display();
    }
}
