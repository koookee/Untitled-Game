using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private int rotateSpeed = 3 * 100;
    private GameManager GameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Prevents player from rotating when inventory's showing
        if (!GameManagerScript.inventoryUI.activeSelf)
        {
            //340 35
            //Limits the range the player can rotate in to look up or down
            float verticalInput = Input.GetAxis("Mouse Y");
            float rotation = transform.rotation.eulerAngles.x;
            if (rotation >= 340 || rotation <= 35)
            {
                transform.Rotate(Vector3.left * Time.deltaTime * verticalInput * rotateSpeed);
            }
            else if (rotation > 35 && rotation <= 187)
            {
                transform.eulerAngles = new Vector3(35, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            else if (rotation < 340 && rotation > 187)
            {
                transform.eulerAngles = new Vector3(340, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            //Debug.Log(transform.rotation.eulerAngles.x);

        }
    }
}
