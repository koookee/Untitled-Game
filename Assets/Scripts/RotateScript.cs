using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private int rotateSpeed = 5 * 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //320 35
        float verticalInput = Input.GetAxis("Mouse Y");
        float rotation = transform.rotation.eulerAngles.x;
        if (rotation >= 340 || rotation <= 35)
        {
            transform.Rotate(Vector3.left * Time.deltaTime * verticalInput * rotateSpeed);
        }
        else if (rotation > 35 && rotation < 150)
        {
            transform.eulerAngles = new Vector3(35, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if (rotation < 340 && rotation > 250)
        {
            transform.eulerAngles = new Vector3(340, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        Debug.Log(transform.rotation.eulerAngles.x);

    }
}
