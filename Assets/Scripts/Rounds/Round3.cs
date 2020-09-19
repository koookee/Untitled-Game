using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round3 : MonoBehaviour
{
    private SpawnManagerScript SpawnManager;
    public int timeBeforeRoundStarts = 5;
    private bool allEnemiesSpawned = false;
    public int roundNum;
    //If copying script, only change RoundX below and in update function
    private Round4 nextRound;
    // Start is called before the first frame update
    void Start()
    {
        SpawnManager = GetComponent<SpawnManagerScript>();
        roundNum = SpawnManager.roundCounter;
        StartCoroutine("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (allEnemiesSpawned)
        {
            //Makes an array to see how many enemies are left
            GameObject[] enemyArr = GameObject.FindGameObjectsWithTag("Enemy");
            //Stops the if condition from being executed if the roundCounter gets incremented
            if (enemyArr.Length == 0 && roundNum == SpawnManager.roundCounter)
            {
                nextRound = GetComponent<Round4>();
                SpawnManager.roundCounter++;
                nextRound.enabled = true;
                //Script turns itself off after its purpose is complete
                enabled = false;
            }
        }
    }
    private IEnumerator Spawner()
    {
        yield return new WaitForSeconds(timeBeforeRoundStarts);
        //1 is regular enemy / 3 is enemy1 in prefabs for spawn manager
        int enemy = 1;
        int enemy1 = 3;
        SpawnManager.enemySpawner(enemy, 5);
        yield return new WaitForSeconds(15);
        SpawnManager.enemySpawner(enemy, 3);
        yield return new WaitForSeconds(5);
        SpawnManager.enemySpawner(enemy1, 1);
        yield return new WaitForSeconds(15);
        SpawnManager.enemySpawner(enemy, 7);
        allEnemiesSpawned = true;
    }
}
