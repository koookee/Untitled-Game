using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private int rotateSpeed = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This is not working
        float verticalInput = Input.GetAxis("Mouse Y");
       // Debug.Log(transform.rotation.y);
        if(transform.rotation.y < 20 && transform.rotation.y > 5)
        {
            transform.Rotate(Vector3.left, verticalInput * Time.deltaTime * rotateSpeed);
        }
    }
    
}
