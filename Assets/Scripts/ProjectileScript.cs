﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 lastPosition;
    private float distanceTraveled;
    public int maxDistance = 25;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletLifeTime();
    }
    private void bulletLifeTime()
    {
        //Determines how far a bullet travels before it respawns
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        if (distanceTraveled > maxDistance)
        {
            //Debug.Log(distanceTraveled);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        knockBackExplosion();
    }

    private void knockBackExplosion()
    {
        //Code used from player ground smash ability
        if (gameObject.tag == "Rocket")
        {
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemyArr)
            {
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                Vector3 awayDirection = (enemyScript.transform.position - transform.position).normalized;
                Vector3 distanceFromPlayer = (enemyScript.transform.position - transform.position);
                //Only knocks back the enemies that are close to the player
                //For extra fun, get rid of the if condition :)
                if (distanceFromPlayer.magnitude < 10f)
                {
                    int knockBackForce = 2;
                    awayDirection = new Vector3(awayDirection.x * knockBackForce, 1, awayDirection.z * knockBackForce);
                    enemyScript.enemyRb.AddForce(awayDirection * 10, ForceMode.Impulse);
                    enemyScript.health -= 3;
                }
            }
            Destroy(gameObject);
        }
    }
}
    