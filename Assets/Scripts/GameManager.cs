using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Camera mainCam;
    public Camera deathCam;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerShieldText;
    public bool isGameOver = false;
    public PlayerController Player;
    public SpawnManagerScript SpawnManager;
    //private GameObject[] enemyArr;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
        mainCam.enabled = true;
        deathCam.enabled = false;
    }

    // Update is called once per frame

    void Update()
    {
        //Updates the player health text
        playerHealthText.text = "Health: " + Player.health;
        playerShieldText.text = "Shield: " + Player.shieldStatus;
        CheckPlayerHealth();
        CheckEnemyHealth();
        
    }
    private void CheckPlayerHealth()
    {
        //Checks if player's still alive
        if (Player.health <= 0)
        {
            deathCam.enabled = true;
            mainCam.enabled = false;
            Player.gameObject.SetActive(false);
            isGameOver = true;
        }
    }
    private void CheckEnemyHealth()
    {
        //Takes an array of enemies present in the world
        GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
        //Loops through the array to check if one of the enemies has health == 0 (basically dead)
        foreach (GameObject enemy in enemyArr)
        {
            //I can't use enemy.health since I need the enemy's script component first
            //This is a quick method of adding the script then checking for the enemy's health
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            if (enemyScript.health == 0)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
