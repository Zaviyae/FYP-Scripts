// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="Wave.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Wave.
/// </summary>
public class Wave  {

    /// <summary>
    /// The wave number
    /// </summary>
    public int waveNum;
    /// <summary>
    /// The enemy count for this wave
    /// </summary>
    public int enemyCount;
    /// <summary>
    /// The concurrent enemies allowed in this wave
    /// </summary>
    public int concEnemy;
    /// <summary>
    /// The types of enemies that can be spawned during this wave
    /// </summary>
    public List<EnemyProfiles> enemies;

    /// <summary>
    /// Initializes a new instance of the wave class.
    /// </summary>
    /// <param name="num">The wave number.</param>
    /// <param name="enemyCount">The concurrent enemies allowed in this wave.</param>
    /// <param name="concEnemy">The concurrent enemies allowed in this wave.</param>
    /// <param name="enemies">The types of enemies that can be spawned during this wave.</param>
    public Wave(int num, int enemyCount, int concEnemy, List<EnemyProfiles> enemies)
    {
        waveNum = num;
        this.enemyCount = enemyCount;
        this.concEnemy = concEnemy;
        this.enemies = enemies;
    }
	
}
