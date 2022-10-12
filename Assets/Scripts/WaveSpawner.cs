using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    
    // Enemies Info
    [Header("Enemies Info")]
    public List<SpawnEnemy> enemies = new List<SpawnEnemy>();
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // Wave Info
    [Header("Wave Info")]
    public int currWave;
    public int waveValue;
    public Transform[] spawnLocations;
    private int spawnIndex;

    // Wave Logic
    [Header("Wave Logic")]
    public int waveDuration;
    public float waveTimer;
    public float spawnInterval;
    public float spawnTimer;

    // Update is called once per frame
    void FixedUpdate()
    {

        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                spawnIndex = Random.Range(0, spawnLocations.Length);

                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocations[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
                ScoreManager.instance.UpdateHUD();
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
            ScoreManager.instance.UpdateHUD();
        }

        if (waveTimer <= 0 && enemiesToSpawn.Count <= 0)
        {
            spawnedEnemies.Clear();
            currWave++;
            ScoreManager.instance.UpdateHUD();
            GenerateWave();
        }
    }

    public void GenerateWave()
    {

        waveValue = currWave * 10;

        if(currWave < 5)
            waveDuration = 40;
        else if(currWave >= 5)
            waveDuration = 60;
        else if(currWave >= 10)
            waveDuration = 90;

        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
        ScoreManager.instance.UpdateHUD();
    }

    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.

        // repeat... 

        //  -> if we have no points left, leave the loop

        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {

            int randEnemyId;

            if(currWave < 5)
                randEnemyId = Random.Range(0, enemies.Count - 2);
            else if(currWave >= 5 && currWave < 8)
                randEnemyId = Random.Range(0, enemies.Count - 1);
            else
                randEnemyId = Random.Range(0, enemies.Count);

            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class SpawnEnemy
{
    public GameObject enemyPrefab;
    public int cost;
}
