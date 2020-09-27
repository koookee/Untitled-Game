﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    private SpawnManagerScript SpawnManager;
    public Rigidbody enemyRb;
    public string enemyType;
    public PlayerController Player;
    public int enemySpeed;
    //maxHealth is a reference for the healthUI to calculate the percentage 
    //of the enemy's current health
    private int maxHealth;
    public int health;
    public float forceApplied;
    private bool inProximityOfPlayer;
    private float proximityDistance = 10f;
    private bool readyToFire = true;
    private int archerReloadTime = 5;
    public GameObject bulletPos;
    public GameObject HealthUI;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
        maxHealth = health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Health();
        MoveTowardsPlayerParent();
        FireAtPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        //When bullet collides with enemy
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Rocket"))
        {
            ParticleSystem bulletParticles = other.gameObject.GetComponent<ParticleSystem>();
            bulletParticles.Play();
            //Destroy(other.gameObject);
            if (other.gameObject.CompareTag("Bullet")) health--;
            //Debug.Log(health);
            //Health reduction from rockets is in the projectile script
        }
        if (other.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
    private void Health()
    {
        //Manages HealthUI of enemies
        if(health != maxHealth)
        {
            //When enemy takes damage, activate their health UI
            HealthUI.SetActive(true);
        }
        //Slider value is based on the fraction value since it's between 0 and 1
        healthSlider.value = (float) health / maxHealth;
        //These two if condition are only for Regular and Bull because when they rotate
        //the health UI rotates with them. The code overrides the rotation around them
        //and sets the position to a point above them.
        if(enemyType == "Regular")
        {
            //Positions the slider above the enemy
            Vector3 position = transform.position;
            healthSlider.transform.position = new Vector3(position.x,position.y + 1,position.z);
            //Makes the slider rotate to face the camera
            healthSlider.transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
        }
        if (enemyType == "Bull")
        {
            //Positions the slider above the enemy
            Vector3 position = transform.position;
            healthSlider.transform.position = new Vector3(position.x, position.y + 2, position.z);
            //Makes the slider rotate to face the camera
            healthSlider.transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
        }
    }
    private void MoveTowardsPlayer()
    {
        //Checks if the player is still alive
        if (Player.gameObject.activeSelf == true)
        {
            //Moves enemy towards player
            Vector3 playerDirection = (Player.transform.position - transform.position).normalized;
            playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);
            enemyRb.AddForce(playerDirection * enemySpeed * forceApplied * 0.01f, ForceMode.VelocityChange);
        }
    }
    private void MoveTowardsPlayerParent()
    {
        //Parent function of moveTowardsPlayer
        if (enemyType == "Regular" || enemyType == "Bull") MoveTowardsPlayer();

        //If archer is in the proximity of the player, it should stop moving
        if (enemyType == "Archer")
        {
            transform.LookAt(GameObject.Find("Player").transform);
            //Calculates distance from archer to player
            Vector3 distanceToPlayer = (Player.transform.position - transform.position);
            if (distanceToPlayer.magnitude < proximityDistance)
            {
                inProximityOfPlayer = true;
                enemyRb.velocity = new Vector3(0, 0, 0);
            }
            else inProximityOfPlayer = false;
            if (!inProximityOfPlayer) MoveTowardsPlayer();
        }
    }
    private void FireAtPlayer()
    {
        //Archer function that fires projectiles at player
        if (inProximityOfPlayer && readyToFire && enemyType == "Archer")
        {
            SpawnManager.bulletSpawner(bulletPos.transform, false);
            StartCoroutine(Timer(archerReloadTime, 1));
        }
    }
    IEnumerator Timer(int timer, int section)
    {
        //Section 1, readyToFire timer cooldown
        if (section == 1)
        {
            Debug.Log("Test");
            readyToFire = false;
            yield return new WaitForSeconds(timer);
            readyToFire = true;
        }
    }
}
