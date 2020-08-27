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
    public EnemyScript Enemy;
    public int health = 10;

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
        /*
        if(health == 0)
        {
            gameObject.SetActive(false);
        }*/
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if player is on the ground to reset doubleJump to 2
        if (other.gameObject.CompareTag("Ground"))
        {
            doubleJump = 2;
        }    
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Calling it from here instead of start because it's only executed once in start 
        Enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        Vector3 awayDirection = (transform.position - Enemy.transform.position).normalized;
        awayDirection = new Vector3(awayDirection.x, 1, awayDirection.z);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerRB.AddForce(awayDirection * 5, ForceMode.Impulse);
            health--;
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
    
}
