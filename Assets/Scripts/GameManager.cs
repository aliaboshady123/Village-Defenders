using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int totalEnemies;
    [SerializeField] int maxEnemiesOnScreen;
    [SerializeField] float spawnWait;
    [SerializeField] float waveWait;

    public static List<GameObject> currentEnemies = new List<GameObject>();
    public int currentEnemiesOnScreen = 0;

    float spawnWaitPassed = 0;
    float waveWaitPassed = 0;
    bool isWaveFinished = false;
    int spawnedEnemiesCount = 0;
    int waveEnemiesCount = 0;
    int enemiesTopSortingLayer = 0;

    void Update()
    {
        SpawnEnemies();
        SetNextWave();
    }

    void SpawnEnemies()
	{
        if(currentEnemiesOnScreen < totalEnemies && !isWaveFinished)
		{
            spawnWaitPassed += Time.deltaTime;
            if (spawnWaitPassed >= spawnWait)
            {
                spawnWaitPassed = 0;
                if (currentEnemiesOnScreen < maxEnemiesOnScreen && spawnedEnemiesCount < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[0], spawnPoint.position, Quaternion.identity);
                    newEnemy.GetComponent<SpriteRenderer>().sortingOrder = enemiesTopSortingLayer;
                    enemiesTopSortingLayer++;
                    currentEnemies.Add(newEnemy);
                    currentEnemiesOnScreen++;
                    spawnedEnemiesCount++;
                    waveEnemiesCount++;
                }
            }
		}
	}

    void SetNextWave()
	{
        if (waveEnemiesCount == maxEnemiesOnScreen)
        {
            isWaveFinished = true;
        }

		if (isWaveFinished && currentEnemiesOnScreen == 0)
		{
            waveWaitPassed += Time.deltaTime;
            if (waveWaitPassed >= waveWait - spawnWait)
			{
                waveEnemiesCount = 0;
                waveWaitPassed = 0;
                isWaveFinished = false;
            }
		}
    }

    public void DecrementEnemies(GameObject enemyObject)
	{
        if(currentEnemiesOnScreen > 0)
		{
            currentEnemies.Remove(enemyObject);
            //currentEnemies.RemoveAt(0);
            currentEnemiesOnScreen--;
		}
	}
}
