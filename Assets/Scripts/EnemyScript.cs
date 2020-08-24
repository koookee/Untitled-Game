using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int health = 4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //When bullet collides with enemy
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health--;
            
        }
    }
}
