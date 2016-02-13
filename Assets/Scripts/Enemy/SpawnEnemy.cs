using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour {
    // creating singleton class
    public static SpawnEnemy instance { get; private set; }

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

    public List<GameObject> spawnSpots;
    public List<EnemyLevel> levels;

    private EnemyLevel currentlevel;

    private bool enemySpawnBlock = true;


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
        if (currentlevel.level < levels.Count-1)
        {
            currentlevel = levels[currentlevel.level + 1];
            currentlevel.enemiesLeft = currentlevel.maxEnemies;
            print("moved to a new level " + currentlevel.level);

        }
    }

    // spawns the enemies
    private void spawnEnemies()
    {
        // stops if all enemies spawned for this level
        if (currentlevel.maxEnemies > currentlevel.spawnedEnemies)
        {       
            // blocking function
            if (enemySpawnBlock)
            {
                enemySpawnBlock = false;

                bool spawned = false;
                int counter = 0;

                while(!spawned)
                {
                    // spawn enemy and deduct the enemy counter to switch the level
                    int spawnspot = Random.Range(0, spawnSpots.Count-1);

                    EnemySpawnArea espArea = spawnSpots[spawnspot].GetComponentInChildren<EnemySpawnArea>();
                    if (!espArea.busy)
                    {
                        spawnEnemy(spawnSpots[spawnspot]);

                        spawned = true;
                        currentlevel.spawnedEnemies++;
                        // if all enemies dead in this level, move on to next one
                        //  if (currentlevel.enemies-- == 0)
                        //  {
                        //      initLevel();
                        //  }
                    }

                    // exits from a loop in case all spawn places busy
                    if (counter++ > 6)
                        break;
                }

                StartCoroutine(spawnWait(currentlevel.delay));
            }

        }

    }

    private void spawnEnemy(GameObject spawnspot)
    {
        Vector2 position = spawnspot.transform.position;

        // randomly choose enemy
        int enemy = Random.Range(0, currentlevel.enemyPrefs.Count);

        // instaciate
        GameObject instance = Instantiate(currentlevel.enemyPrefs[enemy], position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(gameObject.transform);
        MoveEnemy mv = instance.GetComponentInChildren<MoveEnemy>();
        mv.spawnArea = spawnspot;
    }

    private IEnumerator spawnWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        enemySpawnBlock = true;
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
    void Start () {
        initLevel();
	}
	
	// Update is called once per frame
	void Update () {
        spawnEnemies();

    }
}

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