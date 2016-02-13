using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    // creating singleton class
    public static GameManager instance { get; private set; }

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

    public List<EnemyLevel> levels;

    public EnemyLevel currentlevel { get; private set; }



    // initializes the current level and switches to next one
    private void initLevel()
    {
        if (currentlevel == null)
        {
            currentlevel = levels[0];
            currentlevel.enemiesLeft = currentlevel.maxEnemies;
        }

    }

    private void nextLevel()
    {
        //if (enemies[currentlevel.level + 1] != null)
        if (currentlevel.level < levels.Count - 1)
        {
            currentlevel = levels[currentlevel.level + 1];
            currentlevel.enemiesLeft = currentlevel.maxEnemies;
            print("moved to a new level " + currentlevel.level);

        }
    }

    // enemy dies and changes level
    public void enemyDie()
    {

        if (--currentlevel.enemiesLeft <= 0)
        {
            nextLevel();
        }
    }

    // Use this for initialization
    void Start()
    {
        initLevel();
    }
}
