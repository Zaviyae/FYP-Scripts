// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="SpawnManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class SpawnManager.
/// Used for the scene spawning of enemies.
/// </summary>
public class SpawnManager : MonoBehaviour {


    /// <summary>
    /// The enemy prefab
    /// </summary>
    public GameObject enemyPrefab;
    /// <summary>
    /// The inactive enemy pool
    /// </summary>
    public Stack<GameObject>  inactivePool;
    /// <summary>
    /// The active enemy pool
    /// </summary>
    public List<GameObject> activePool;
    /// <summary>
    /// The enemies remaining
    /// </summary>
    public int enemiesRemaining;
    /// <summary>
    /// The spawn time
    /// </summary>
    public float spawnTime;

    /// <summary>
    /// The player
    /// </summary>
    public Player player;

    /// <summary>
    /// The current spawn room array
    /// </summary>
    public GameObject[] currentSpawnRoomArray;
    /// <summary>
    /// The spawn rooms for level 1
    /// </summary>
    public GameObject[] spawnRooms;
    /// <summary>
    /// The spawn rooms for level 2
    /// </summary>
    public GameObject[] spawnRooms2;

    /// <summary>
    /// The fireball prefab
    /// </summary>
    public GameObject fireballPrefab;
    /// <summary>
    /// The fireball pool
    /// </summary>
    public ObjectPool fireballPool;

    /// <summary>
    /// The shield wave text component. (Third party Text component)
    /// </summary>
    public TMPro.TextMeshProUGUI shieldWaveText;

    /// <summary>
    /// The available spawn rooms
    /// </summary>
    List<GameObject> availableSpawns;
    /// <summary>
    /// The unavailable spawn rooms
    /// </summary>
    List<GameObject> unavailableSpawns;


    /// <summary>
    /// The available levels
    /// </summary>
    public List<Level> levels;

    /// <summary>
    /// The current level
    /// </summary>
    public Level currentLevel;
    /// <summary>
    /// The current wave
    /// </summary>
    public Wave currentWave;

    /// <summary>
    /// The level 2 portal
    /// </summary>
    public GameObject Level2Portal;
    /// <summary>
    /// Trigger to start the level (after player has taken portal)
    /// </summary>
    bool levelTriggered;

    /// <summary>
    /// The animators for a flying enemy and land enemy.
    /// </summary>
    public RuntimeAnimatorController landAnim, flyAnim;

    /// <summary>
    /// Starts this instance.
    /// Sets up all waves and levels
    /// </summary>
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

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        shieldWaveText = player.rInput.shield.GetComponent<Shield>().wave;
        StartWave(currentWave);
        StartCoroutine(WaveCheck());
        
    }
    /// <summary>
    /// Called once per frame,
    /// Sets element shields on enemies based on player element type.
    /// </summary>
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


    /// <summary>
    /// Starts the wave.
    /// </summary>
    /// <param name="wave">The wave.</param>
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


        StartCoroutine(SpawnTime(2f));
        

    }

    /// <summary>
    /// Waits for time before spawning an enemy.
    /// </summary>
    /// <param name="time">The time.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator SpawnTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnEnemy();
        
    }

    /// <summary>
    /// Trigger the next level.
    /// </summary>
    public void LevelTrigger()
    {
        levelTriggered = true;

        if (currentSpawnRoomArray == spawnRooms)
        {
            currentSpawnRoomArray = spawnRooms2;
        }
    }
    /// <summary>
    /// Spawns the enemy if all conditions are met.
    /// </summary>
    void SpawnEnemy()
    {
        if (activePool.Count <= currentWave.concEnemy && enemiesRemaining > 0)
        {
            EnemyProfiles[] enemyTypeArray = currentWave.enemies.ToArray();

            if (availableSpawns.Count > 0)
            {
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

    /// <summary>
    /// Removes a spawn room from unavailable spawn if the enemy has died.
    /// </summary>
    /// <param name="e">The e.</param>
    public void removeMe(EnemySpawn e)
    {
        unavailableSpawns.Remove(e.gameObject);
        availableSpawns.Add(e.gameObject);
       
    }

    /// <summary>
    /// Calls the CheckWave method every 5 seconds.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator WaveCheck()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(5);
            CheckWave();
        }
    }
    /// <summary>
    /// Checks the wave to see if all enemies are dead and the player has beat the wave.
    /// </summary>
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

    /// <summary>
    /// Called if an enemy has died, handles enemy pool.
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    public void EnemyDeceased(GameObject enemy)
    {
        inactivePool.Push(enemy);
        activePool.Remove(enemy);
        enemy.SetActive(false);
       
    }

    /// <summary>
    /// Returns the active enemy pool to who ever calls this.
    /// </summary>
    /// <returns>List&lt;GameObject&gt;.</returns>
    public List<GameObject> inform()
    {
        return activePool;
    }
}
