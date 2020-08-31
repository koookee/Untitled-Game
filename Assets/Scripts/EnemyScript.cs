using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody enemyRb;
    public string enemyType;
    public PlayerController Player;
    public int enemySpeed = 3;
    // Start is called before the first frame update
    public int health = 4;
    public float forceApplied = 1f;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        }
    }
    private void moveTowardsPlayer()
    {
        //Checks if the player is still alive
        if (Player.gameObject.activeSelf == true)
        {
            //Moves enemy towards player
            Vector3 playerDirection = (Player.transform.position - transform.position).normalized;
            playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z);
            enemyRb.AddForce(playerDirection * enemySpeed * forceApplied *0.01f , ForceMode.VelocityChange);
        }
    }
}
