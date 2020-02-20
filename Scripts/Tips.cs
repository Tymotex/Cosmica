using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour {
    /* Tips:
     * 10% of the score you achieve in each level becomes credits! The higher your score, the more credits you will earn
     * Purchase upgrades to your ships in the shop!
     * You get 400 credits for each level unlocked for the first time!
     * When enemies and defenders collide, they deal massive damage to each other!
     * Destroyed enemies have a chance to drop coins!
     * New ships can be unlocked as you progress through more levels!
     * 
     */
    public string[] tips;  // Manually write individual tips as elements of this array
    [SerializeField] Text tipsText = null;
    [SerializeField] string tipsPrefix;

    private void Start() {
        int randomIndex = Random.Range(0, tips.Length);
        DisplayTip(randomIndex);
    }

    private void DisplayTip(int index) {
        tipsText.text = tipsPrefix + tips[index];
    }
}
