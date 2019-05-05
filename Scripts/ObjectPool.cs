// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="ObjectPool.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class ObjectPool. Used for pooling certain objects such as the enemy fire balls.
/// </summary>
public class ObjectPool  {

    /// <summary>
    /// The inactive pool
    /// </summary>
    public Stack<GameObject> inactivePool;
    /// <summary>
    /// The active pool
    /// </summary>
    public List<GameObject> activePool;
    /// <summary>
    /// The object to pool
    /// </summary>
    public GameObject myObject;

    GameObject empty;
    /// <summary>
    /// The start number
    /// </summary>
    int startNum;


    /// <summary>
    /// Initializes a new instance of the <see cref=".ObjectPool"/> class.
    /// </summary>
    /// <param name="obj">The object to pool.</param>
    /// <param name="startNumber">The start number.</param>
    public ObjectPool(GameObject obj, int startNumber)
    {
        startNum = startNumber;
        inactivePool = new Stack<GameObject>();
        activePool = new List<GameObject>();

        empty = GameObject.Instantiate(new GameObject());
        empty.name = obj.name + " Pool";

        for(int i = 0; i <= startNumber; i++)
        {
            push(GameObject.Instantiate(obj));
        }

        

    }


    /// <summary>
    /// Returns object on the top of inactive pool, or spawns a new one.
    /// </summary>
    /// <returns>GameObject.</returns>
    public GameObject get()
    {
        if (inactivePool.Count > 0 && inactivePool.Peek())
        {
            checkUp();
            GameObject popped = inactivePool.Pop();
            popped.transform.parent = null;
            activePool.Add(popped);
 
            
            return popped;
        }
        else
        {
            return GameObject.Instantiate(myObject);
        }

    }

    /// <summary>
    /// Push gameobject to the inactive pool.
    /// </summary>
    /// <param name="temp">The temporary gameobject.</param>
    void push(GameObject temp)
    {
        temp.AddComponent<DisableAfter>();
        temp.GetComponent<DisableAfter>().time = 10;
        temp.transform.parent = empty.transform;
        temp.SetActive(false);
        inactivePool.Push(temp);
    }

    /// <summary>
    /// Checks all game objects in active pool to make sure they are active.
    /// </summary>
    void checkUp()
    {
        
            foreach (GameObject o in activePool.ToArray())
            {
                if (!o.activeSelf)
                {
                
                    push(o);
                    activePool.Remove(o);
                    
                }
            }
 
    }
}
