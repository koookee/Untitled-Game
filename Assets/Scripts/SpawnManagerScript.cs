using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public PlayerController PlayerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletSpawner();
    }
    public void bulletSpawner()
    {
        //Spawns bullets from the player's side
        if (Input.GetMouseButtonDown(0))
        {
            //Bullets spawn at the invisible object location (BulletPos)
            GameObject BulletPos = GameObject.FindGameObjectWithTag("BulletPos");
            Instantiate(prefabs[0], BulletPos.transform.position, PlayerControllerScript.transform.rotation);
        }
    }
}
