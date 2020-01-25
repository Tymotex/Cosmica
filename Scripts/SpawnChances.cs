using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnChances {
    [Tooltip("Manually set all the spawn chances (needs to sum to 100). This is part of the level design")]
    public int[] spawnChances;
}
