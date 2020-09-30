using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 lastPosition;
    public GameObject[] prefabs;
    private float distanceTraveled;
    public int maxDistance = 25;
    private Vector3 moveDirection = new Vector3(0, 0, 1);
    public int speed = 5;
    public AudioSource rocketExplosionSound;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletLifeTime();
        ProjectileMove();
    }
    private void ProjectileMove()
    {
        transform.Translate(moveDirection * Time.deltaTime * speed);
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
    private void OnTriggerEnter(Collider other)
    {
        knockBackExplosion();
    }

    private void knockBackExplosion()
    {
        //Code used from player ground smash ability
        if (gameObject.tag == "Rocket")
        {
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
                    enemyScript.health -= 3;
                }
            }
            //prefabs[0] is the rocket explosion
            //Added +1 to the y position because explosion kept spawning in the ground
            //instead of above
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject Explosion = Instantiate(prefabs[0], position, transform.rotation);
            //Destroys the explosion game object after 3 seconds
            Destroy(Explosion, 3);
            Destroy(gameObject);
        }
    }
}
    
