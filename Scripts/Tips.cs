using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour {
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
   /* Tips:
    * 10% of the score you achieve in each level becomes credits! The higher your score, the more credits you will earn
    * Purchase upgrades to your ships in the shop!
    * You get 400 credits for each level unlocked for the first time!
    * When enemies and defenders collide, they deal massive damage to each other!
    * Destroyed enemies have a chance to drop coins!
    * New ships can be unlocked as you progress through more zones and levels!
    * You can toggle the background music in the options menu!
    * Check the stats of the possible enemies to plan your battle!
    * Different defender ships have different stats. Protect your weaker units!
    * Enemies spawn faster the further you are into the level!
    * Tougher enemies start spawning the longer you stay in the level!
    * Tougher enemies give you greater rewards!
    * The less energy you spend on a level, the higher your score!
    * You get bonus score points for finishing a level before the timer ends!
    * You will lose some control and score points when your own units die. Protect them!
    * Sentinel and Fortress are great front-line units!
    * Lasereye and Scimitar are fragile but deal high damage!
    * A higher defence value reduces the damage taken by a unit!
    * You have 500 energy to start each level. Use it wisely!
    */
