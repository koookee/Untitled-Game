using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManagerScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public PlayerController PlayerControllerScript;
    public GameManager GameManagerScript;
    private int enemySpawnTimer = 5;
    private int healthPackSpawnTimer = 10;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine("spawnEnemies");
        StartCoroutine("spawnHealthPacks");
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
            //Bullets spawn at the invisible object location attached to player(BulletPos)
            GameObject BulletPos = GameObject.FindGameObjectWithTag("BulletPos");
            Instantiate(prefabs[0], BulletPos.transform.position, PlayerControllerScript.transform.rotation);
        }
    }
    public void enemySpawner()
    {
        //Spawns enemies in this field of range
        float enemyRangeX = UnityEngine.Random.Range(-30, 30);
        float enemyRangeY = UnityEngine.Random.Range(-30, 30);
        Vector3 enemySpawnLocation = new Vector3(enemyRangeX, 1, enemyRangeY);
        Instantiate(prefabs[1], enemySpawnLocation,prefabs[1].transform.rotation);
    }
    public void healthSpawner()
    {
        //Spawns health packs
        float healthPackRangeX = UnityEngine.Random.Range(-40, 40);
        float healthPackRangeY = UnityEngine.Random.Range(-40, 40);
        Vector3 healthPackSpawnLocation = new Vector3(healthPackRangeX, 0.63f, healthPackRangeY);
        Instantiate(prefabs[2], healthPackSpawnLocation, prefabs[2].transform.rotation);     
    }
    private IEnumerator spawnEnemies()
    {
        //Waits 5 seconds to spawn a new enemy
        while (!GameManagerScript.isGameOver)
        {
            enemySpawner();
            yield return new WaitForSeconds(enemySpawnTimer);       
        }
    }
    private IEnumerator spawnHealthPacks()
    {
        while (!GameManagerScript.isGameOver)
        {
            //Spawns health packs every 10 seconds
            healthSpawner();
            yield return new WaitForSeconds(healthPackSpawnTimer);
        }
    }
    
}
