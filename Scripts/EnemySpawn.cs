// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="EnemySpawn.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class EnemySpawn. Script attached to all spawn rooms in the scene.
/// </summary>
public class EnemySpawn : MonoBehaviour {

    /// <summary>
    /// Is this spawn active.
    /// </summary>
    public bool active;

    /// <summary>
    /// The spawn location.
    /// </summary>
    public Transform spawn;
    /// <summary>
    /// The current enemy on this spawn.
    /// </summary>
    public GameObject currentEnemy;
    /// <summary>
    /// The spawn manager
    /// </summary>
    public SpawnManager spawnManager;

    /// <summary>
    /// Is this a fly spot.
    /// </summary>
    public bool flySpot;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        currentEnemy = null;
        StartCoroutine(Check());

	}


    /// <summary>
    /// Checks every three seconds to see if enemy is inactive. If so, reports to spawn manager.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Check()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(3);
            if (currentEnemy != null)
            {
                if (!currentEnemy.activeSelf)
                {
                    currentEnemy = null;
                    spawnManager.removeMe(this);
                }
            }
        }
    }


}
