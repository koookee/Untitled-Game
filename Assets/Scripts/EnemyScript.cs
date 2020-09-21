using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private SpawnManagerScript SpawnManager;
    public Rigidbody enemyRb;
    public string enemyType;
    public PlayerController Player;
    public int enemySpeed = 3;
    public int health = 4;
    public float forceApplied = 1f;
    private bool inProximityOfPlayer;
    private float proximityDistance = 10f;
    public bool readyToFire = true;
    private int archerReloadTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManagerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForHealth();
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
            Debug.Log(health);
            //Health reduction from rockets is in the projectile script
        }
        if (other.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
    private void CheckForHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
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
            SpawnManager.bulletSpawner(transform, false);
            StartCoroutine(Timer(archerReloadTime, 1));
        }
    }
    IEnumerator Timer(int timer, int section)
    {
        //Section 1, readyToFire timer cooldown
        if (section == 1)
        {
            readyToFire = false;
            yield return new WaitForSeconds(timer);
            readyToFire = true;
        }
    }
}
