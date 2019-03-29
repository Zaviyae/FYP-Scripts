using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {


    public int[] waveAmount;
    public GameObject enemyPrefab;
    public Stack<GameObject>  inactivePool;
    public List<GameObject> activePool;
    public int enemiesRemaining;
    public float spawnTime;

    public Player player;
    public List<EnemyProfiles> availableEnemyTypes;

    public GameObject[] currentSpawnRoomArray;
    public GameObject[] spawnRooms;
    public GameObject[] spawnRooms2;

    public GameObject fireballPrefab;
    public ObjectPool fireballPool;

   // public TMPro.TextMeshPro waveText;
    public TMPro.TextMeshProUGUI shieldWaveText;

    List<GameObject> availableSpawns;
    List<GameObject> unavailableSpawns;


    public List<Level> levels;

    public Level currentLevel;
    public Wave currentWave;

    public GameObject Level2Portal;
    bool levelTriggered;

    public RuntimeAnimatorController landAnim, flyAnim;

    void Start () {
        currentSpawnRoomArray = spawnRooms;

        List<EnemyProfiles> waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        Wave waveOne = new Wave(1, 10, 4, waveEnemyList);


        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        Wave waveTwo = new Wave(2, 15, 5, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        Wave waveThree = new Wave(3, 20, 5, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        Wave waveFour = new Wave(4, 30, 6, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        waveEnemyList.Add(new EnemyProfiles.Healer(2));
        Wave waveFive = new Wave(5, 35, 7, waveEnemyList);

        List<Wave> currentWaveList = new List<Wave>();
        currentWaveList.Add(waveOne);
        currentWaveList.Add(waveTwo);
        currentWaveList.Add(waveThree);
        currentWaveList.Add(waveFour);
        currentWaveList.Add(waveFive);
        currentWaveList.Add(null);

        Level level1 = new Level(1,currentWaveList);
        levels = new List<Level>();

        levels.Add(level1);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveOne = new Wave(1, 10, 4, waveEnemyList);


        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        waveTwo = new Wave(2, 15, 5, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        waveEnemyList.Add(new EnemyProfiles.Healer(2));

        waveThree = new Wave(3, 20, 5, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Mage(3));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(3));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        waveEnemyList.Add(new EnemyProfiles.Healer(2));

        waveFour = new Wave(4, 30, 6, waveEnemyList);

        waveEnemyList = new List<EnemyProfiles>();
        waveEnemyList.Add(new EnemyProfiles.Archer(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(1));
        waveEnemyList.Add(new EnemyProfiles.Mage(2));
        waveEnemyList.Add(new EnemyProfiles.Mage(3));
        waveEnemyList.Add(new EnemyProfiles.Archer(2));
        waveEnemyList.Add(new EnemyProfiles.Archer(3));
        waveEnemyList.Add(new EnemyProfiles.Healer(1));
        waveEnemyList.Add(new EnemyProfiles.Healer(2));
        waveEnemyList.Add(new EnemyProfiles.Healer(3));

        waveFive = new Wave(5, 40, 7, waveEnemyList);

        currentWaveList = new List<Wave>();
        currentWaveList.Add(waveOne);
        currentWaveList.Add(waveTwo);
        currentWaveList.Add(waveThree);
        currentWaveList.Add(waveFour);
        currentWaveList.Add(waveFive);
        currentWaveList.Add(null);

        Level level2 = new Level(2, currentWaveList);

        levels.Add(level2);
        levels.Add(null);

  
        fireballPool = new ObjectPool(fireballPrefab, 100);

        activePool = new List<GameObject>();
        inactivePool = new Stack<GameObject>();



        currentLevel = levels[0];
        currentWave = currentLevel.waves[0];





        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	public void StartGame()
    {
        shieldWaveText = player.rInput.shield.GetComponent<Shield>().wave;
        StartWave(currentWave);
        StartCoroutine(WaveCheck());
        
    }
	void Update () {

        ElementType.Type playerElement = player.elementType;

        foreach (GameObject e in activePool)
        {
            if(e.GetComponent<Enemy>().elementType == playerElement)
            {
                e.GetComponent<Enemy>().ElementShield(true);
            }
            else
            {
                e.GetComponent<Enemy>().ElementShield(false);
            }
        }
	}


    public void StartWave(Wave wave)
    {
        shieldWaveText.text = "WAVE : " + (wave.waveNum);
        availableSpawns = new List<GameObject>();
        unavailableSpawns = new List<GameObject>();

        foreach (GameObject g in currentSpawnRoomArray)
        {
            g.GetComponent<EnemySpawn>().currentEnemy = null;
            g.GetComponent<EnemySpawn>().spawnManager = this;
            availableSpawns.Add(g);
           
        }

        player.currentHealth = player.maxHealth;

        enemiesRemaining = currentWave.enemyCount;

        //enemiesRemaining = waveAmount[waveNumber];

        StartCoroutine(SpawnTime(2f));
        

    }

    IEnumerator SpawnTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
        
    }

    public void LevelTrigger()
    {
        levelTriggered = true;

        if (currentSpawnRoomArray == spawnRooms)
        {
            currentSpawnRoomArray = spawnRooms2;
        }
    }
    void SpawnEnemy()
    {
        if (activePool.Count <= currentWave.concEnemy && enemiesRemaining > 0)
        {
            EnemyProfiles[] enemyTypeArray = currentWave.enemies.ToArray();

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

                if (chosenSpawn.flySpot)
                {
                    spawnedEnemy.GetComponent<Animator>().runtimeAnimatorController = flyAnim;
                }
                else
                {
                    spawnedEnemy.GetComponent<Animator>().runtimeAnimatorController = landAnim;
                }
                spawnedEnemy.GetComponent<Enemy>().anim = spawnedEnemy.GetComponent<Animator>();
                spawnedEnemy.GetComponent<Enemy>().SetUp(enemyTypeArray[Random.Range(0, enemyTypeArray.Length)]);
                spawnedEnemy.GetComponent<Enemy>().spawnManager = this;
                spawnedEnemy.SetActive(true);

                enemiesRemaining--;

                chosenSpawn.currentEnemy = spawnedEnemy;
                availableSpawns.Remove(chosenSpawn.gameObject);
                unavailableSpawns.Add(chosenSpawn.gameObject);

            }
        }

            StartCoroutine(SpawnTime(Random.Range(2, 8)));
        

    }

    public void removeMe(EnemySpawn e)
    {
        unavailableSpawns.Remove(e.gameObject);
        availableSpawns.Add(e.gameObject);
       
    }

    IEnumerator WaveCheck()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(5);
            CheckWave();
        }
    }
    void CheckWave()
    {

        if(enemiesRemaining == 0 && activePool.Count == 0)
        {
            print("wave complete");

            if (currentLevel.waves[currentWave.waveNum] != null)  //dont need to +1
            {
                currentWave = currentLevel.waves[currentWave.waveNum];
                StartWave(currentWave);
            }
            else
            {
                print("level complete...");
                if (levels[currentLevel.levelnum] != null)
                {
                    if (levelTriggered)
                    {
                        currentLevel = levels[currentLevel.levelnum];
                        currentWave = currentLevel.waves[0];
                        levelTriggered = false;
                        StartWave(currentWave);
                    }
                    Level2Portal.SetActive(true);
                    
                }
                else
                {
                    print("game finished.");
                    StopAllCoroutines();
                }
            }
            
            
            
        }
    }

    public void Deceased(GameObject enemy)
    {
        inactivePool.Push(enemy);
        activePool.Remove(enemy);
        enemy.SetActive(false);
       
    }

    public List<GameObject> inform()
    {
        return activePool;
    }
}
