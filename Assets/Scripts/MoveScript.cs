using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    //Edit this depending on which direction you want an object to move in
    private Vector3 moveDirection = new Vector3(0, 0, 1);
    public int speed = 5;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime * speed);
    }
}
