using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManagerScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public PlayerController PlayerControllerScript;
    public GameManager GameManagerScript;
    private int healthPackSpawnTimer = 10;
    GameObject BulletPos;
    //Rounds:
    public int roundCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        BulletPos = GameObject.FindGameObjectWithTag("BulletPos");
        StartCoroutine("spawnHealthPacks");
    }

    // Update is called once per frame
    void Update()
    {
        bulletSpawner(BulletPos.transform, true);
    }
    public void bulletSpawner(Transform transform, bool isThisFromPlayer)
    {
        //Transform transform has all the information about the game object's
        //position, rotation, etc.
        //isThisFromPlayer checks to see if it's bullets spawned by player or enemy archer
        if (isThisFromPlayer)
        {
            //Spawns bullets from the player's side
            //If the player isn't viewing their inventory, they can shoot
            if (Input.GetMouseButtonDown(0) && !GameManagerScript.inventoryUI.activeSelf)
            {
                //Bullets spawn at the invisible object location attached to player (BulletPos)
                if (PlayerControllerScript.weaponSelected == "Gun")
                {
                    Instantiate(prefabs[0], transform.position, transform.rotation);
                    //Plays the shooting sound effect
                    PlayerControllerScript.AudioClips[0].Play();
                }
                if (PlayerControllerScript.weaponSelected == "Rocket Launcher" && PlayerControllerScript.rocketAmmo > 0)
                {
                    Instantiate(prefabs[4], transform.position, transform.rotation);
                    PlayerControllerScript.rocketAmmo--;
                    PlayerControllerScript.ammo.text = "Ammo: " + PlayerControllerScript.rocketAmmo;
                }
            }
        }
        else
        {
            Instantiate(prefabs[5], transform.position, transform.rotation);
        }
    }
    public void enemySpawner(int enemyType, int amount)
    {
        //x determines what enemy to spawn
        //Spawns enemies in this field of range
        for (int i = 0; i < amount; i++)
        {
            //Spawns an int amount of enemy prefabs 
            float enemyRangeX = UnityEngine.Random.Range(-30, 30);
            float enemyRangeZ = UnityEngine.Random.Range(-30, 30);
            Vector3 enemySpawnLocation = new Vector3(enemyRangeX, 2, enemyRangeZ);
            Instantiate(prefabs[enemyType], enemySpawnLocation, prefabs[enemyType].transform.rotation);
        }
    }
    public void healthSpawner()
    {
        //Spawns health packs
        float healthPackRangeX = UnityEngine.Random.Range(-40, 40);
        float healthPackRangeY = UnityEngine.Random.Range(-40, 40);
        Vector3 healthPackSpawnLocation = new Vector3(healthPackRangeX, 0.63f, healthPackRangeY);
        Instantiate(prefabs[2], healthPackSpawnLocation, prefabs[2].transform.rotation);     
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

    //Switched to a round approach
    /*
    private IEnumerator spawnEnemies()
    {
        //Waits 5 seconds to spawn a new enemy
        while (!GameManagerScript.isGameOver)
        {
            enemySpawner(1);
            yield return new WaitForSeconds(enemySpawnTimer);       
        }
    }
    private IEnumerator spawnEnemies1()
    {
        //Waits 30 seconds to spawn a new enemy
        while (!GameManagerScript.isGameOver)
        {
            enemySpawner(3);
            yield return new WaitForSeconds(enemySpawnTimer1);
        }
    }
    */
}
