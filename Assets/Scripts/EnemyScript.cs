using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody enemyRb;
    public GameObject Player;
    private int enemySpeed = 3;
    // Start is called before the first frame update
    private int health = 4;
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player");
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
        }
    }
    private void moveTowardsPlayer()
    {
        //Moves enemy towards player
        Vector3 playerDirection = (Player.transform.position - transform.position).normalized;
        enemyRb.AddForce(playerDirection * enemySpeed * 0.01f, ForceMode.VelocityChange);
    }
}
