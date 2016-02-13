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


    private GameManager gameManager;
    public List<GameObject> spawnSpots;
    private bool enemySpawnBlock = true;


    // spawns the enemies
    private void spawnEnemies()
    {
        // stops if all enemies spawned for this level
        if (gameManager.currentlevel.maxEnemies > gameManager.currentlevel.spawnedEnemies)
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
                        gameManager.currentlevel.spawnedEnemies++;

                    }

                    // exits from a loop in case all spawn places busy
                    if (counter++ > 6)
                        break;
                }

                StartCoroutine(spawnWait(gameManager.currentlevel.delay));
            }

        }

    }

    private void spawnEnemy(GameObject spawnspot)
    {
        Vector2 position = spawnspot.transform.position;

        // randomly choose enemy
        int enemy = Random.Range(0, gameManager.currentlevel.enemyPrefs.Count);

        // instaciate
        GameObject instance = Instantiate(gameManager.currentlevel.enemyPrefs[enemy], position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(gameObject.transform);
        MoveEnemy mv = instance.GetComponentInChildren<MoveEnemy>();
        mv.spawnArea = spawnspot;
    }

    private IEnumerator spawnWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        enemySpawnBlock = true;
    }


    // Use this for initialization
    void Start () {
        gameManager = GameManager.instance;
    }
	
	// Update is called once per frame
	void Update () {
        spawnEnemies();

    }
}
