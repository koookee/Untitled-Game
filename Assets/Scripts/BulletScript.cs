using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 lastPosition;
    private float distanceTraveled;
    private int maxDistance = 25;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletLifeTime();
    }
    private void bulletLifeTime()
    {
        //Determines how far a bullet travels before it respawns
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        if (distanceTraveled > maxDistance)
        {
            //Debug.Log(distanceTraveled);
            Destroy(gameObject);
        }
    }
}
