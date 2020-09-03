using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float vertical = 0f;
    private float horizontal = 0f;
    private Rigidbody playerRB;
    private float playerSpeed = 2.0f;
    private float jumpForce = 5.0f;
    private int doubleJump = 2;
    private int rotateSpeed = 9 * 100;
    public int health = 10;
    //Shield 
    private bool isShieldActive = false;
    private bool shieldCooledDown = true;
    public string shieldStatus = "Ready";
    private int shieldDuration = 10;
    private int shieldCoolDown = 3;
    public float shieldDurationTimer = 10f;
    public float coolDownTimer = 3f;
    //Shield 

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MoveFunc();
        JumpFunc();
        ShieldActivation(shieldDuration,shieldCoolDown);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if player is on the ground to reset doubleJump to 2
        if (other.gameObject.CompareTag("Ground"))
        {
            doubleJump = 2;
        }    
        if(other.gameObject.CompareTag("Health Pack"))
        {
            health++;
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Calling it from here instead of start because it's only executed once in start 
        //It takes a random Enemy, though, not the one closest to the player
        //Enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        if (collision.gameObject.CompareTag("Enemy") && !isShieldActive)
        {
            //This gets the script of the collider the player collides with
            EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
            Vector3 awayDirection = (transform.position - Enemy.transform.position).normalized;
            awayDirection = new Vector3(awayDirection.x, 1, awayDirection.z);
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

    private void JumpFunc()
    {
        //Takes the input to make the player jump
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump > 0)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJump -= 1;
        }
    }
    private void ShieldActivation(int shieldDuration, int shieldCoolDown)
    {
        //Activates shield when shield isn't cooling down & when player presses 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && shieldCooledDown)
        {
            StartCoroutine(ShieldTimer(shieldDuration, shieldCoolDown));
            //Resets timers whenever player presses 1
            shieldDurationTimer = (float)shieldDuration;
            coolDownTimer = (float)shieldCoolDown;
        }
        if (shieldStatus == "Active") shieldDurationTimer -= Time.deltaTime;
        if (shieldStatus == "Cooling down") coolDownTimer -= Time.deltaTime;
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
        //Rotates player around the Y axis
        float horizontalInput = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * rotateSpeed);
    }
    IEnumerator ShieldTimer(int shieldDuration, int shieldCoolDown)
    {
        shieldCooledDown = false;
        isShieldActive = true;
        shieldStatus = "Active";
        yield return new WaitForSeconds(shieldDuration);
        isShieldActive = false;
        shieldStatus = "Cooling down";
        yield return new WaitForSeconds(shieldCoolDown);
        shieldCooledDown = true;
        shieldStatus = "Ready";
    }
    
}
