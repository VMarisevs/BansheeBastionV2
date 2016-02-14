using UnityEngine;
using System.Collections;

public class PlaceTower : MonoBehaviour {

    public GameObject towerPrefab;
    private GameObject tower;



    private bool canPlaceTower()
    {
        return tower == null;
    }

    private bool canUpgradeTower()
    {
        if (tower != null)
        {
            TowerData towerData = tower.GetComponent<TowerData>();
            TowerLevel nextLevel = towerData.getNextLevel();
            if (nextLevel != null)
            {
                return true;
            }
        }
        return false;
    }

    

    void OnMouseUp()
    {
        if (canPlaceTower())
        {
       //     print("y:" + gameObject.transform.position.y);
       //     SpriteRenderer sp = towerPrefab.GetComponent<Tower>();
       //     sp.sortingOrder = (int)gameObject.transform.position.y;

            foreach(Transform child in towerPrefab.transform)
            {
                SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
                sp.sortingOrder = -((int)gameObject.transform.position.y);
            }


            tower = (GameObject) Instantiate(towerPrefab, transform.position, Quaternion.identity);

            GameManager gm = GameManager.instance;
            gm.addTower(tower);


            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

         //   SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
         //   sp.sortingOrder = (int)gameObject.transform.position.y;

            // TODO: Deduct gold
        }
        else if (canUpgradeTower())
        {
            tower.GetComponent<TowerData>().increaseLevel();
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);
            // TODO: Deduct gold
        }

    }

}
