// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-03-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-03-2019
// ***********************************************************************
// <copyright file="shootpoint.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class shootpoint. Helper script used for aiming at player.
/// </summary>
public class shootpoint : MonoBehaviour {
    /// <summary>
    /// The player
    /// </summary>
    Player player;


    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}


    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
        transform.LookAt(player.transform);
	}
}
