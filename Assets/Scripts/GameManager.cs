﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera mainCam;
    public Camera deathCam;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerShieldText;
    public TextMeshProUGUI playerGroundShatterText;
    public TextMeshProUGUI roundNum;
    public TextMeshProUGUI doubleJumpStatus;
    public TextMeshProUGUI roundsSurvived;
    //Using Image instead of GameObject doesn't make it show up in the GameManager
    //game object like TextMeshProUGUI does
    public GameObject rayGunImage;
    public GameObject rocketLauncherImage;
    public GameObject crossHair;
    public GameObject weaponsUI;
    public GameObject restartButton;
    public bool isGameOver = false;
    public PlayerController Player;
    public SpawnManagerScript SpawnManager;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
        mainCam.enabled = true;
        deathCam.enabled = false;
        //Cursor.visible also works but I'm getting an ambiguous error between UnityEngine
        //and UnityEngine.UI
        UnityEngine.Cursor.visible = false;
        //Keeps the mouse in the playmode area
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame

    void Update()
    {
        displayUI();
        CheckPlayerHealth();
        CheckEnemyHealth();
        
    }
    private void displayUI()
    {
        abilitiesUI();
        weaponUI();
        roundUI();
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
            RestartGameUI();
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
    private void abilitiesUI()
    {
        //Updates the player health text
        playerHealthText.text = "Health: " + Player.health;
        //Updates the player shield text
        if (Player.shieldStatus == "Active")
        {
            //Added the + 1 because the timer goes from 9 to 0 instead of 10 to 1
            int timer = (int)Player.shieldDurationTimer + 1;
            playerShieldText.text = "Shield: " + Player.shieldStatus + ", " + timer;
        }
        if (Player.shieldStatus == "Cooling down")
        {
            //Added the + 1 because the timer goes from 2 to 0 instead of 3 to 1
            int timer = (int)Player.coolDownTimer + 1;
            playerShieldText.text = "Shield: " + Player.shieldStatus + ", " + timer; ;
        }
        if (Player.shieldStatus == "Ready") playerShieldText.text = "Shield: " + Player.shieldStatus;
        if (Player.groundSmashStatus != "Ready")
        {
            int timer = (int)Player.groundSmashCoolDown + 1;
            playerGroundShatterText.text = "Ground shatter:" + timer;
        }
        else
        {
            playerGroundShatterText.text = "Ground shatter: Ready";
        }
    }
    private void weaponUI()
    {
        if (Player.weaponSelected == "Gun")
        {
            rayGunImage.SetActive(true);
            rocketLauncherImage.SetActive(false);
        }
        if (Player.weaponSelected == "Rocket Launcher")
        {
            rocketLauncherImage.SetActive(true);
            rayGunImage.SetActive(false);
        }
    }
    private void roundUI()
    {
        //Displays round info, pop up messages, and anything to do with the 
        //changing state of the game
        roundNum.text = "Round: " + SpawnManager.roundCounter;
        if (doubleJumpStatus.gameObject.activeSelf == true) StartCoroutine(Timer(4,1));
    }
    private void RestartGameUI()
    {
        //Turns off all UI first
        playerHealthText.gameObject.SetActive(false);
        playerShieldText.gameObject.SetActive(false);
        playerGroundShatterText.gameObject.SetActive(false);
        roundNum.gameObject.SetActive(false);
        rayGunImage.gameObject.SetActive(false);
        rocketLauncherImage.gameObject.SetActive(false);
        crossHair.gameObject.SetActive(false);
        weaponsUI.gameObject.SetActive(false);
        //Turns on restart screen UI
        roundsSurvived.gameObject.SetActive(true);
        int roundSurvivedNum = SpawnManager.roundCounter - 1;
        roundsSurvived.text = "Rounds survived: " + roundSurvivedNum;
        restartButton.gameObject.SetActive(true);
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        //Turns off all UI first
        playerHealthText.gameObject.SetActive(true);
        playerShieldText.gameObject.SetActive(true);
        playerGroundShatterText.gameObject.SetActive(true);
        roundNum.gameObject.SetActive(true);
        rayGunImage.gameObject.SetActive(true);
        rocketLauncherImage.gameObject.SetActive(true);
        crossHair.gameObject.SetActive(true);
        weaponsUI.gameObject.SetActive(true);
        //Turns on restart screen UI
        roundsSurvived.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }
    IEnumerator Timer(int time, int section)
    {
        //Manages different commands in one coroutine split up into sections 
        //If you want to execute a few lines of code for a few seconds,
        //add a new section and pass that parameter.
        //This is more organized than having multiple IEnumerator functions
        if (section == 1)
        {
            yield return new WaitForSeconds(time);
            doubleJumpStatus.gameObject.SetActive(false);
        }
    }
}

