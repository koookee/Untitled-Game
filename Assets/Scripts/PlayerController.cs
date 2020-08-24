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
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if player is on the ground to reset doubleJump to 2
        if (other.gameObject.CompareTag("Ground"))
        {
            doubleJump = 2;
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
