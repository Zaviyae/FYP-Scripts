using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public int wave;
    public int[] waveAmount;
    public GameObject enemyPrefab;
    public Stack<GameObject>  inactivePool;
    public List<GameObject> activePool;
    public int enemiesRemaining;
    public float spawnTime;
    public Transform spawnCornerx, spawnCornery;
    public Player player;
    public List<EnemyProfiles> availableEnemyTypes;
    public GameObject[] spawnRooms;

	void Start () {
       // RenderSettings.fogDensity = 0.06f;
        activePool = new List<GameObject>();
        inactivePool = new Stack<GameObject>();

        availableEnemyTypes = new List<EnemyProfiles>();
        availableEnemyTypes.Add(new EnemyProfiles.Archer(1));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(2));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(3));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(4));

        StartWave(0);
	}
	
	
	void Update () {
		
	}


    public void StartWave(int waveNumber)
    {
        player.currentHealth = player.maxHealth;
        enemiesRemaining = waveAmount[waveNumber];

        StartCoroutine(SpawnTime(2f));

    }

    IEnumerator SpawnTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        EnemyProfiles[] enemyTypeArray = availableEnemyTypes.ToArray();
        
       // int randomx = Random.Range(-42, 42);
        //int randomz = Random.Range(41, 125);
        //Vector3 spawnLoc = new Vector3(randomx, 0, randomz);
        Vector3 spawnLoc = spawnRooms[Random.Range(0, spawnRooms.Length)].GetComponent<EnemySpawn>().spawn.position;
        GameObject spawnedEnemy;

        if (inactivePool.Count >= 1)
        {
            spawnedEnemy = inactivePool.Pop();  
            activePool.Add(spawnedEnemy);
            spawnedEnemy.transform.position = spawnLoc;
        }
        else
        {
            print(inactivePool.Count);
            spawnedEnemy = GameObject.Instantiate(enemyPrefab, spawnLoc, Quaternion.identity);
            activePool.Add(spawnedEnemy);
        }

        spawnedEnemy.GetComponent<Enemy>().SetUp(enemyTypeArray[Random.Range(0, enemyTypeArray.Length)]);
        spawnedEnemy.GetComponent<Enemy>().spawnManager = this;
        spawnedEnemy.SetActive(true);

        if(enemiesRemaining != 1)
        {
            StartCoroutine(SpawnTime(Random.Range(2,8)));
        }
        enemiesRemaining--;

       
    }

    void CheckWave()
    {
        if(enemiesRemaining == 0 && activePool.Count == 0)
        {
            print("wave complete");
            wave++;
            StartWave(wave);
        }
    }

    public void Deceased(GameObject enemy)
    {
        inactivePool.Push(enemy);
        activePool.Remove(enemy);
        enemy.SetActive(false);
        CheckWave();
    }
}
