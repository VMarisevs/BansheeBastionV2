using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyLevel
{
    public int maxEnemies;

    public float delay;
    public int level;
    public List<GameObject> enemyPrefs;

    public int spawnedEnemies = 0;
    public int enemiesLeft;
}
