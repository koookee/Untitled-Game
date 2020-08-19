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
            Vector3 bulletPos = new Vector3(PlayerControllerScript.transform.position.x + 1, PlayerControllerScript.transform.position.y, PlayerControllerScript.transform.position.z);
            Instantiate(prefabs[0], bulletPos, prefabs[0].transform.rotation);
        }
    }
}
