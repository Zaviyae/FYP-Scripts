// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Elemental.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Elemental. For the simple AI companion to the player.
/// </summary>
public class Elemental : MonoBehaviour {

    /// <summary>
    /// The element type of this elemental
    /// </summary>
    public ElementType.Type elementType;
    /// <summary>
    /// The enemy target
    /// </summary>
    public GameObject target = null;
    /// <summary>
    /// The potential targets
    /// </summary>
    public GameObject[] potentials;
    /// <summary>
    /// The filtered targets
    /// </summary>
    public List<GameObject> filtered;

    /// <summary>
    /// The move point
    /// </summary>
    public Vector3 movePoint;
    /// <summary>
    /// The target start
    /// </summary>
    bool targetStart;
    /// <summary>
    /// The player
    /// </summary>
    public GameObject player;
    /// <summary>
    /// If we use a custom skill
    /// </summary>
    public bool useCustomSkill;
    /// <summary>
    /// The life time
    /// </summary>
    public float lifeTime;
    /// <summary>
    /// If the elemental is close to target
    /// </summary>
    public bool close;

    /// <summary>
    /// Is flamethrower active
    /// </summary>
    public bool flamethrower;
    /// <summary>
    /// The flame prefab
    /// </summary>
    public GameObject flame;
  

    /// <summary>
    /// The skills array
    /// </summary>
    public Skill[] skills;

    /// <summary>
    /// Starts this instance.
    /// Begins co routines and randomises life time.
    /// </summary>
    void Start () {
        flamethrower = true;

        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("Check");
        StartCoroutine("Fire");

        if(lifeTime == 0)
        {
            lifeTime = UnityEngine.Random.Range(25, 80);
        }
        StartCoroutine(Destroy());
        StartCoroutine(FlameTick());
    }

 
    /// <summary>
    /// Called once a frame. 
    /// Handles movement to enemy.
    /// </summary>
    void FixedUpdate () {
        flame.SetActive(close);
        if (target)
        {
            targetStart = false;
            transform.LookAt(target.transform);
            if (Vector3.Distance(transform.position, target.transform.position) <= 4)
            {
                Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, pos, 0.2f * Time.deltaTime);
                close = true;
                
            }
            else
            {
                close = false;
                Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, pos, 0.7f * Time.deltaTime);
            }

        }
        else
        {
            close = false;
            if (!targetStart)
            {
                StartCoroutine(RandomMove());
            }
            targetStart = true;

            transform.position = Vector3.Lerp(transform.position, movePoint, 0.5f * Time.deltaTime);
        }
	}

    /// <summary>
    /// Creates random movement for the elemental.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator RandomMove()
    {
        while (!target)
        {
            int sec = UnityEngine.Random.Range(5, 15);
 

            float randomx = UnityEngine.Random.Range(player.transform.position.x - 10, player.transform.position.x + 10);
            float randomz = UnityEngine.Random.Range(player.transform.position.z - 1, player.transform.position.z + 10);

            movePoint = new Vector3(randomx, transform.position.y, randomz);
            Search();
            yield return new WaitForSeconds(sec);
        }
        
    }

    /// <summary>
    /// Check to see we have target every .2 seconds.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Check()
    {
        for(; ; )
        {
            if (target != null && !target.activeSelf)
            {
                target = null;
            }

            Search();
            yield return new WaitForSeconds(.2f);
        }
    
    }


    /// <summary>
    /// Damage tick for flame thrower effect. Applies damage to the enemy.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator FlameTick()
    {
        for (; ; )
        {
            if (flamethrower)
            {
                if (target != null)
                {
                    if (target.GetComponent<Enemy>().currentHealth <= 0) target = null;
                }
                yield return new WaitForSeconds(0.15f);
                if (close)
                {
                    Color damageColor = Color.black;
                    switch (elementType)
                    {
                        case ElementType.Type.Blue:
                            damageColor = Color.blue;
                            break;
                        case ElementType.Type.Red:
                            damageColor = Color.yellow;
                            break;
                        case ElementType.Type.Purple:
                            damageColor = Color.magenta;
                            break;
                    }
                    target.GetComponent<Enemy>().TakeDamage(1, damageColor);
                }
            }
        }
    }

    /// <summary>
    /// Use custom skill on enemy.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Fire()
    {
        for (; ; )
        {
            
            if (target)
            {
                if (useCustomSkill)
                {
                    Enemy currentTarget = target.GetComponent<Enemy>();
                    currentTarget.spawnObject(skills[0], false);

                }
                else
                {

                }

                
            }
            float time = UnityEngine.Random.Range(7, 20);
            yield return new WaitForSeconds(time);
        }
    }

    /// <summary>
    /// Destroys this instance after time is up.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifeTime);
        if (target)
            target.GetComponent<Enemy>().lockedOn = false;
        Destroy(gameObject);
    }
    /// <summary>
    /// Searches for enemies in the scene we can go to.
    /// </summary>
    void Search()
    {
  
        potentials = GameObject.FindGameObjectsWithTag("Enemy");

        if (potentials.Length == 0)
        {

        }
        else
        {
            try
            {
                filtered = new List<GameObject>();

                foreach (GameObject g in potentials)
                {
                    if (!g.GetComponent<Enemy>().lockedOn)
                    {
                        filtered.Add(g);
                    }

                }

                if (target)
                {
                    //nothing
                }
                else
                {
                    int randomEnemy = UnityEngine.Random.Range(0, filtered.Count);
                    target = filtered[randomEnemy];
                    target.GetComponent<Enemy>().LockOn();
                }

            }catch(Exception e)
            {

            }
        }
    }
}
