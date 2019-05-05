// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="Tornado.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Tornado. A trivial class, used only for one 'Ultimate' ability that can be used in the game.
/// </summary>
public class Tornado : MonoBehaviour {

    /// <summary>
    /// The time left
    /// </summary>
    public int time;
    /// <summary>
    /// The player
    /// </summary>
    Player player;
    /// <summary>
    /// The spawn manager
    /// </summary>
    public SpawnManager spawn;
    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
	}

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
		
	}

    /// <summary>
    /// Summons this instance.
    /// </summary>
    public void Summon()
    {
        StartCoroutine(CountDown());
        StartCoroutine(DamageTick());
    }

    /// <summary>
    /// Counts down the time left on the tornado.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    /// <summary>
    /// Ticks damage to all nearby enemies every .4 seconds.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator DamageTick()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(.4f);
            foreach(GameObject e in spawn.activePool)
            {
                e.GetComponent<Enemy>().TakeDamage(3, Color.yellow);
            }
        }
        
    }

}
