using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave  {

    public int waveNum;
    public int enemyCount;
    public int concEnemy;
    public List<EnemyProfiles> enemies;

    public Wave(int num, int enemyCount, int concEnemy, List<EnemyProfiles> enemies)
    {
        waveNum = num;
        this.enemyCount = enemyCount;
        this.concEnemy = concEnemy;
        this.enemies = enemies;
    }
	
}
