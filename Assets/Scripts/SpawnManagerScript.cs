using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public PlayerController PlayerControllerScript;
    public GameManager GameManagerScript;
    private int enemySpawnTimer = 5;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine("spawnEnemies");
    }

    // Update is called once per frame
    void Update()
    {
        bulletSpawner();
    }
    public void bulletSpawner()
    {
        //Spawns bullets from the player's side
        if (Input.GetMouseButtonDown(0))
        {
            //Bullets spawn at the invisible object location (BulletPos)
            GameObject BulletPos = GameObject.FindGameObjectWithTag("BulletPos");
            Instantiate(prefabs[0], BulletPos.transform.position, PlayerControllerScript.transform.rotation);
        }
    }
    public void enemySpawner()
    {
        //Spawns enemies in this field of range
        float randomNum = Random.Range(-15, 15);
        Vector3 enemySpawnLocation = new Vector3(randomNum, 1, randomNum);
        Instantiate(prefabs[1], enemySpawnLocation,PlayerControllerScript.transform.rotation);
    }
    private IEnumerator spawnEnemies()
    {
        //Waits 5 seconds to spawn a new enemy
        while (!GameManagerScript.isGameOver)
        {
            yield return new WaitForSeconds(enemySpawnTimer);
            enemySpawner();
        }
    }
}
