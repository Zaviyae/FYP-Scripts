// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-22-2019
//
// Last Modified By : zaviy
// Last Modified On : 02-22-2019
// ***********************************************************************
// <copyright file="SpawnItem.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class SpawnItem.
/// Used for spawning a power-up ball to shoot at player.
/// </summary>
public class SpawnItem : MonoBehaviour {

    /// <summary>
    /// Third party script reference used for effect control.
    /// </summary>
    RFX4_ScaleCurves scaleCurves;
    /// <summary>
    /// The time before shooting the ball.
    /// </summary>
    float time;
    /// <summary>
    /// Is the power-up ball currently active.
    /// </summary>
    bool objFloat;
    /// <summary>
    /// The ball spawn prefab
    /// </summary>
    public GameObject spawnPrefab;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {

        transform.root.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        scaleCurves = GetComponent<RFX4_ScaleCurves>();
        time = scaleCurves.GraphTimeMultiplier / 2;

	}

    /// <summary>
    /// Sets up the portal, looking at the players location.
    /// </summary>
    public void setUp()
    {
        transform.root.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        scaleCurves = GetComponent<RFX4_ScaleCurves>();
        time = scaleCurves.GraphTimeMultiplier / 2;
        objFloat = false;
    }
    /// <summary>
    /// If the time has passed, release the ball.
    /// </summary>
    void Update () {

        if (time <= 0 && !objFloat)
        {
            Release();
        }
        else
        {
            time -= 1 * Time.fixedDeltaTime;
        }



	}

    /// <summary>
    /// Releases the power-up ball.
    /// </summary>
    void Release()
    {
        GameObject obj = Instantiate(spawnPrefab, transform.position, transform.rotation, transform);
        objFloat = true;
        print("released!");
        obj.SetActive(true);
        
        obj.GetComponent<PowerUpThrow>().floating();
    }
}
