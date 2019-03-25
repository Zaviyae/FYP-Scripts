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

    public Player player;
    public List<EnemyProfiles> availableEnemyTypes;
    public GameObject[] spawnRooms;

    public GameObject fireballPrefab;
    public ObjectPool fireballPool;


    List<GameObject> availableSpawns;
    List<GameObject> unavailableSpawns;

    void Start () {

        fireballPool = new ObjectPool(fireballPrefab, 100);

        activePool = new List<GameObject>();
        inactivePool = new Stack<GameObject>();

        availableEnemyTypes = new List<EnemyProfiles>();
        availableEnemyTypes.Add(new EnemyProfiles.Archer(1));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(2));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(3));
        availableEnemyTypes.Add(new EnemyProfiles.Archer(4));

        availableEnemyTypes.Add(new EnemyProfiles.Mage(1));
        availableEnemyTypes.Add(new EnemyProfiles.Mage(2));
        availableEnemyTypes.Add(new EnemyProfiles.Mage(3));
        availableEnemyTypes.Add(new EnemyProfiles.Mage(4));

        StartWave(0);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	
	void Update () {
		
	}


    public void StartWave(int waveNumber)
    {
        availableSpawns = new List<GameObject>();
        unavailableSpawns = new List<GameObject>();

        foreach (GameObject g in spawnRooms)
        {
            g.GetComponent<EnemySpawn>().currentEnemy = null;
            g.GetComponent<EnemySpawn>().spawnManager = this;
            availableSpawns.Add(g);
           
        }

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

        if (availableSpawns.Count > 0)
        {
            // Vector3 spawnLoc = spawnRooms[Random.Range(0, spawnRooms.Length)].GetComponent<EnemySpawn>().spawn.position;
            EnemySpawn chosenSpawn = availableSpawns[Random.Range(0, availableSpawns.Count)].GetComponent<EnemySpawn>();
           
            GameObject spawnedEnemy;

            
            if (inactivePool.Count >= 1)
            {
                spawnedEnemy = inactivePool.Pop();
                activePool.Add(spawnedEnemy);
                spawnedEnemy.transform.position = chosenSpawn.spawn.position; 
            }
            else
            {
                print(inactivePool.Count);
                spawnedEnemy = GameObject.Instantiate(enemyPrefab, chosenSpawn.spawn.position, Quaternion.identity);
                activePool.Add(spawnedEnemy);
            }


            spawnedEnemy.GetComponent<Enemy>().SetUp(enemyTypeArray[Random.Range(0, enemyTypeArray.Length)]);
            spawnedEnemy.GetComponent<Enemy>().spawnManager = this;
            spawnedEnemy.SetActive(true);

            if (enemiesRemaining != 1)
            {
                StartCoroutine(SpawnTime(Random.Range(2, 8)));
            }
            enemiesRemaining--;

            chosenSpawn.currentEnemy = spawnedEnemy;
            availableSpawns.Remove(chosenSpawn.gameObject);
            unavailableSpawns.Add(chosenSpawn.gameObject);

        }


    }

    public void removeMe(EnemySpawn e)
    {
        unavailableSpawns.Remove(e.gameObject);
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

    public List<GameObject> inform()
    {
        return activePool;
    }
}
