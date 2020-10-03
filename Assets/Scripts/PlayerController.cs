using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager GameManagerScript;
    private float vertical = 0f;
    private float horizontal = 0f;
    private Rigidbody playerRB;
    private float playerSpeed = 2.0f;
    private float jumpForce = 5.0f;
    public int numOfJumps = 1;
    private int jumpsLeft;
    private int rotateSpeed = 9 * 100;
    private bool isOnGround;
    public int health = 10;
    public int gems = 0;
    private bool isMaxOnHealth = true;
    public AudioSource[] AudioClips;
    private ParticleSystem particles;
    private Renderer Mesh;
    private Color shieldColor = new Color(0.050f, 0.342f, 1.108f, 1.000f);
    private Color whiteColor =new Color(1,1,1,1);
    private Color coolingDownColor = new Color(0.755f, 0.224f, 0.082f, 1.000f);
    //Weapon Arr
    public TextMeshProUGUI ammo;
    public string[] weaponArr = new string[] {"Gun","Rocket Launcher"};
    public string weaponSelected = "";
    public int rocketAmmo = 5; 
    //Weapon num is basically just the weapon array index
    private int weaponNum = 0;
    //Abilities:
    //Shield 
    private bool isInvincible = false;
    private bool isShieldActive = false;
    private bool shieldCooledDown = true;
    public string shieldStatus = "Ready";
    private int shieldDuration = 10;
    private int shieldCoolDown = 3;
    public float shieldDurationTimer = 10f;
    public float coolDownTimer = 3f;
    //Ground Smash
    private bool isGroundSmash = false;
    public float groundSmashCoolDown = 3f;
    public float groundSmashCoolDownTimer = 3;
    private bool playerInvincible = false;
    //Game starts when ability is cooling down
    public string groundSmashStatus = "cooling down";


    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        weaponSelected = weaponArr[weaponNum];
        Mesh = GetComponent<Renderer>();
        playerRB = GetComponent<Rigidbody>();
        particles = GetComponent<ParticleSystem>();
        //Used it to check color RGB values at the start of the game
        //Color color = Mesh.material.GetColor("_Color");
        //Debug.Log(color);
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MoveFunc();
        JumpFunc();
        ShieldActivation(shieldDuration,shieldCoolDown);
        GroundSmash();
        WeaponSelector();
        Health();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health Pack") && !isMaxOnHealth)
        {
            //Plays the heal sound effect
            AudioClips[1].Play();
            health++;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            //If the shield isn't active, the player loses health
            if (!isInvincible) health--;
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Checks to see if player is on the ground to reset doubleJump to 2
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpsLeft = numOfJumps;
        }
        
        //Calling it from here instead of start because it's only executed once in start 
        //It takes a random Enemy, though, not the one closest to the player
        //Enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        if (collision.gameObject.CompareTag("Enemy") && !isShieldActive && !playerInvincible)
        {
            //This gets the script of the collider the player collides with
            EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
            Vector3 awayDirection = (transform.position - Enemy.transform.position).normalized;
            awayDirection = new Vector3(awayDirection.x, 1, awayDirection.z);
            AudioClips[4].Play();
            if(Enemy.enemyType == "Regular")
            {
                playerRB.AddForce(awayDirection * 5, ForceMode.Impulse);
                health--;
            }
            if(Enemy.enemyType == "Bull")
            {
                playerRB.AddForce(awayDirection * 10, ForceMode.Impulse);
                health = health - 3;
                //If health goes below 0. It adjusts it back so that the player health
                //UI doesn't display negative numbers
                if(health < 0)
                {
                    health = 0;
                }
            }
            
        }

        //Knocks enemies back and gives player a shield
        if (collision.gameObject.CompareTag("Enemy") && isShieldActive)
        {
            //This gets the script of the collider the player collides with
            EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
            Vector3 awayDirection = (Enemy.transform.position - transform.position ).normalized;
            awayDirection = new Vector3(awayDirection.x, 1, awayDirection.z);
            AudioClips[4].Play();
            if (Enemy.enemyType == "Regular")
            {
                Enemy.enemyRb.AddForce(awayDirection * 5, ForceMode.Impulse);
            }
            if (Enemy.enemyType == "Bull")
            {
                Enemy.enemyRb.AddForce(awayDirection * 6, ForceMode.Impulse);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") isOnGround = false;
    }

    private void JumpFunc()
    {
        //Takes the input to make the player jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsLeft -= 1;
        }
    }
    private void ShieldActivation(int shieldDuration, int shieldCoolDown)
    {
        //Activates shield when shield isn't cooling down & when player presses 1
        //Shield knocks enemies back when they collide with player
        if (Input.GetKeyDown(KeyCode.Alpha1) && shieldCooledDown)
        {
            AudioClips[3].Play();
            isInvincible = true;
            StartCoroutine(ShieldTimer(shieldDuration, shieldCoolDown));
            //Resets timers whenever player presses 1
            shieldDurationTimer = (float)shieldDuration;
            coolDownTimer = (float)shieldCoolDown;
        }
        if (shieldStatus == "Active") shieldDurationTimer -= Time.deltaTime;
        if (shieldStatus == "Cooling down") coolDownTimer -= Time.deltaTime;
    }
    private void GroundSmash()
    {
        //When player isn't contact with the ground, they can press 2 to
        //smash the ground knocking all enemies back

        //Manages cooldown timer
        groundSmashCoolDown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isOnGround && groundSmashCoolDown <= 0)
        { 
            //doubleJump is another way of showing whether the player is on the ground or not
            //doubleJump of value 2 means the player is on the ground
            isGroundSmash = true;
            //Prevents enemies from knocking back player while he's diving down 
            playerInvincible = true;
            playerRB.AddForce(Vector3.down * 20, ForceMode.Impulse);
            //Resets timer
            groundSmashCoolDown = groundSmashCoolDownTimer;
            //Changes status but it's not displayed
            groundSmashStatus = "cooling down";

        }
        if (isOnGround && isGroundSmash)
        {
            AudioClips[2].Play();
            particles.Play();
            //Takes an array of all the enemies in the map
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemyArr)
            {
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                Vector3 awayDirection = (enemyScript.transform.position - transform.position).normalized;
                Vector3 distanceFromPlayer = (enemyScript.transform.position - transform.position);
                //Only knocks back the enemies that are close to the player
                //For extra fun, get rid of the if condition argument :)
                if (distanceFromPlayer.magnitude < 10f)
                {
                    if (enemyScript.enemyType == "Archer") enemyScript.hasBeenLaunched = true;
                    int knockBackForce = 2;
                    awayDirection = new Vector3(awayDirection.x * knockBackForce, 1, awayDirection.z * knockBackForce);
                    enemyScript.enemyRb.AddForce(awayDirection * 10, ForceMode.Impulse);
                }
            }
            isGroundSmash = false;
            playerInvincible = false;
        }
        if(groundSmashCoolDown <= 0)
        {
            groundSmashStatus = "Ready";
        }
    }
    private void MoveFunc()
    {
        //Makes the player move left and right
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * Time.deltaTime * vertical * playerSpeed);
        transform.Translate(Vector3.right * Time.deltaTime * horizontal * playerSpeed);
    }
    private void RotatePlayer()
    {
        //If inventory is open, player can't rotate
        if (!GameManagerScript.inventoryUI.activeSelf)
        {
            //Rotates player around the Y axis
            float horizontalInput = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * rotateSpeed);
        }
    }
    private void WeaponSelector()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && weaponNum < weaponArr.Length - 1)
        {
            weaponNum++;
            weaponSelected = weaponArr[weaponNum];
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && weaponNum > 0)
        {
            weaponNum--;
            weaponSelected = weaponArr[weaponNum];
        }
    }
    IEnumerator ShieldTimer(int shieldDuration, int shieldCoolDown)
    {
        shieldCooledDown = false;
        isShieldActive = true;
        shieldStatus = "Active";
        //Changes mesh color
        Mesh.material.SetColor("_Color", shieldColor);
        yield return new WaitForSeconds(shieldDuration);
        Mesh.material.SetColor("_Color", coolingDownColor);
        isShieldActive = false;
        shieldStatus = "Cooling down";
        isInvincible = false;
        yield return new WaitForSeconds(shieldCoolDown);
        Mesh.material.SetColor("_Color", whiteColor);
        shieldCooledDown = true;
        shieldStatus = "Ready";
    }
    private void Health()
    {
        //Checks to see if health is 5 to set isMaxOnHealth to true
        //This prevents the player from going over 10 HP
        if (health == 5) isMaxOnHealth = true;
        else isMaxOnHealth = false;
    }
}
