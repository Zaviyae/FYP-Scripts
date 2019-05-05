// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="GameManager.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class GameManager. Handles spawning power-up portals on a timer.
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// The power-up portal prefab
    /// </summary>
    public GameObject portalPrefab;
    /// <summary>
    /// The portal spawn time
    /// </summary>
    public float portalSpawnTime;

    /// <summary>
    /// The position to spawn the portal.
    /// </summary>
    public Transform point;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        
        StartCoroutine(RandomSpawn());
	}

 
    /// <summary>
    /// Spawns randomly on a timer.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator RandomSpawn()
    {
        for(; ; )
        {
            if(portalSpawnTime > 0f)
            {
                yield return new WaitForSeconds(portalSpawnTime);

            }
            else
            {
                yield return new WaitForSeconds(Random.Range(20, 80));
            }


            GameObject port = Instantiate(portalPrefab, point.position, portalPrefab.transform.rotation);
            SpawnItem portalSpawn = port.GetComponentInChildren<SpawnItem>();

            portalSpawn.setUp();
        }
    }
}
