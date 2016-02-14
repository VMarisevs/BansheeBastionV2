using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public float speed = 1.0f;
    public GameObject spawnArea;

    public GameObject target;

    public Vector3 targetPos;

    private float lastWaypointSwitchTime;

    private bool initTarget()
    {
        if (target == null)
        {
            GameManager gm = GameManager.instance;

            if (gm.towersList.Count > 0)
            {
                int targetId = Random.Range(0, gm.towersList.Count - 1);
                target = gm.towersList[targetId];
                print("switch target");
            } else
                target = GameObject.FindGameObjectsWithTag("Castle")[0];


            float[,] around = new float[5, 2] { { 0, 0 }, { 0, 1 }, { -0.5f, 0 }, { -0.5f, 0.5f }, { -0.5f, 1 } };

            int pos = Random.Range(0, around.Length/2 - 1);
            print(target.transform.position +" " + around[pos, 0]);
            targetPos = new Vector3(target.transform.position.x + around[pos, 0], target.transform.position.y + around[pos, 1]);

            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
        lastWaypointSwitchTime = Time.time;
        //initTarget();
    }
	
	// Update is called once per frame
	void Update () {
        move();

    }

    private void move()
    {
        //    try
        //    {
        if (!initTarget())
        {
            Vector3 startPosition = spawnArea.transform.position;



            //Vector3 endPositiona = targetPos;//target.transform.position;

            float pathLength = Vector3.Distance(startPosition, targetPos);
            float totalTimeForPath = pathLength / speed;
            float currentTimeOnPath = Time.time - lastWaypointSwitchTime;


            gameObject.transform.position = Vector3.Lerp(startPosition, targetPos, currentTimeOnPath / totalTimeForPath);

            changeSort();

            if (gameObject.transform.position.Equals(targetPos))
            {


                // 3.b 
                // Destroy(gameObject);

                //   AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                //   AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                // TODO: deduct health

            }
        }
       
    //    }
    //    catch(System.Exception e)
    //    {
    //        initTarget();
    //    }
    //
        
    }

    private void changeSort()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sortingOrder = -((int)gameObject.transform.position.y);
       // print("current position: " +  gameObject.transform.position.y);
    }

    void OnDestroy()
    {
        GameManager gameManager = GameManager.instance;
        gameManager.enemyDie();

    }
}
