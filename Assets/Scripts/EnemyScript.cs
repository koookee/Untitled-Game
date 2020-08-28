﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody enemyRb;
    public PlayerController Player;
    private int enemySpeed = 3;
    // Start is called before the first frame update
    public int health = 4;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
        moveTowardsPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        //When bullet collides with enemy
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health--;
            Debug.Log(health);
        }
    }
    private void moveTowardsPlayer()
    {
        //Checks if the player is still alive
        //NOT WORKING!!!!!
        if (Player.gameObject.activeSelf == true)
        {
            //Moves enemy towards player
            Vector3 playerDirection = (Player.transform.position - transform.position).normalized;
            enemyRb.AddForce(playerDirection * enemySpeed * 0.01f, ForceMode.VelocityChange);
        }
    }
}
