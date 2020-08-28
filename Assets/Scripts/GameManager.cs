using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Camera mainCam;
    public Camera deathCam;
    public TextMeshProUGUI playerHealthText;
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
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = "Health:" + " " + Player.health; 
        CheckPlayerHealth();
    }
    private void CheckPlayerHealth()
    {
        //Checks if player's still alive
        if (Player.health == 0)
        {
            deathCam.enabled = true;
            mainCam.enabled = false;
            Player.gameObject.SetActive(false);
            isGameOver = true;
        }
    }
}
