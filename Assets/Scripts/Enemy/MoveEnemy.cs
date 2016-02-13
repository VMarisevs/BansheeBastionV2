using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    public float speed = 1.0f;
    public GameObject spawnArea;

    public GameObject target;

    private float lastWaypointSwitchTime;

    private void initTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectsWithTag("Castle")[0];
        }
    }

    // Use this for initialization
    void Start () {
        lastWaypointSwitchTime = Time.time;
        initTarget();
    }
	
	// Update is called once per frame
	void Update () {
        move();

    }

    private void move()
    {
        Vector3 startPosition = spawnArea.transform.position;
        Vector3 endPosition = target.transform.position;

        float pathLength = Vector3.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;


        gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        changeSort();

        if (gameObject.transform.position.Equals(endPosition))
        {


            // 3.b 
            Destroy(gameObject);

            //   AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            //   AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            // TODO: deduct health

        }
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
