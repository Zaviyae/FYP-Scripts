// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-20-2019
//
// Last Modified By : zaviy
// Last Modified On : 02-21-2019
// ***********************************************************************
// <copyright file="Wandcollider.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Wandcollider. Used for targetting enemies.
/// When an enemy enters the wand trigger it is added, when it leaves the trigger it's removed.
/// </summary>
public class Wandcollider : MonoBehaviour {

    /// <summary>
    /// The enemies in the trigger
    /// </summary>
    List<GameObject> enemies;
    /// <summary>
    /// the chosen Enemy target
    /// </summary>
    GameObject temptarget, target;
    /// <summary>
    /// The player
    /// </summary>
    public Player player;
    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        enemies = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

    // Update is called once per frame
    /// <summary>
    /// Checks the current enemies in the wand trigger to find which enemy is closest to the player, this then becomes the target.
    /// Also handles removing the target if the enemy no longer is active.
    /// </summary>
    void FixedUpdate () {

        //Clears target if the target is inactive.
        if (target)
        {
            if (!target.GetComponent<Enemy>().isActiveAndEnabled)
            {
                enemies.Remove(target);
            }
        }


        float smallestdist = Mathf.Infinity;
        if (enemies.Count == 0)
            return;
		foreach(GameObject o in enemies)
        {
            if(Vector3.Distance(transform.position, o.transform.position) < smallestdist)
            {
                smallestdist = Vector3.Distance(transform.position, o.transform.position);

                if (o.GetComponent<Enemy>())
                {
                    temptarget = o.gameObject;
                }
                else
                {
                    try
                    {
                        temptarget = o.transform.root.gameObject;
                    }catch(Exception e)
                    {

                    }
                }
                
                
            }
        }
        if (target == null)
        {
            target = temptarget;
            target.GetComponent<Enemy>().targetted = true;
        }
        else
        {
            target.GetComponent<Enemy>().targetted = false;
            target = temptarget;
            target.GetComponent<Enemy>().targetted = true;
        }
        player.target = target;
    }

    /// <summary>
    /// Called when an object enters the trigger
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject);
        }
        else
        {
            if (collision.transform.root.tag == "Enemy")
            {
                enemies.Add(collision.transform.root.gameObject);
            }
            else
            {
                if (target)
                {
                    target.GetComponent<Enemy>().targetted = false;
                    target = null;
                }
            }

        }
    }

    /// <summary>
    /// Called when an object exits the trigger
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnTriggerExit(Collider collision)
    {


        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Remove(collision.gameObject);
        }
        else
        {
            if (collision.transform.root.tag == "Enemy")
            {
                enemies.Remove(collision.transform.root.gameObject);
            }
            else
            {
                if (target && enemies.Count == 0)
                {
                    target.GetComponent<Enemy>().targetted = false;
                    target = null;
                }
            }

        }
    }
}
