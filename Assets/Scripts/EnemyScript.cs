using System.Collections;
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
    //Checks to see if archer has been launched by a rocket
    public bool hasBeenLaunched = false;
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
        
        if (other.gameObject.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //When bullet collides with enemy
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Rocket"))
        {
            ParticleSystem bulletParticles = collision.gameObject.GetComponent<ParticleSystem>();
            bulletParticles.Play();
            //Destroy(other.gameObject);
            if (collision.gameObject.CompareTag("Bullet")) health--;
            //Debug.Log(health);
            //Health reduction from rockets is in the projectile script
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
            if (distanceToPlayer.magnitude < proximityDistance && !hasBeenLaunched)
            {
                inProximityOfPlayer = true;
                enemyRb.velocity = new Vector3(0, 0, 0);
            }
            //If an archer has been hit by a rocket, they'll float away into space  
            //and never be seen again (self-destruct)
            else if (hasBeenLaunched == true) StartCoroutine(Timer(5, 2));
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
            readyToFire = false;
            yield return new WaitForSeconds(timer);
            readyToFire = true;
        }
        //Section 2, Archer launched by rocket
        //after a certain period of time (timer) of floating
        //in space, Archer will self-destruct
        if (section == 2)
        {
            yield return new WaitForSeconds(timer);
            //Since the player killed the archer by launching them into space, they won't
            //receive gems by reducing their health 0. This ensures the player receives
            //gems no matter what their method of destroying the archer was
            Player.gems += 40;
            Destroy(gameObject);
            //gameObject being Archer
        }
    }
}
