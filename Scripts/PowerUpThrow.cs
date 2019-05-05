// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-22-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-23-2019
// ***********************************************************************
// <copyright file="PowerUpThrow.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class PowerUpThrow.
/// Attached to the power-up ball shot at the player.
/// </summary>
public class PowerUpThrow : MonoBehaviour {

    /// <summary>
    /// The y axis
    /// </summary>
    public float yAxis;
    /// <summary>
    /// The elemental array
    /// </summary>
    public GameObject[] elemental;
    /// <summary>
    /// The sphere object of this ball.
    /// </summary>
    public GameObject sphere;
    /// <summary>
    /// If the ball has been thrown or has made contact with the shield.
    /// </summary>
    public bool thrown, shieldcontact;
    /// <summary>
    /// The elemental identifier
    /// </summary>
    public int elementalID;
    /// <summary>
    /// If the elemental spawned is random or is set.
    /// </summary>
    public bool random,set;
    /// <summary>
    /// If the ball is mid-air.
    /// </summary>
    public bool objFloat;
    /// <summary>
    /// The rigidbody attached to the sphere.
    /// </summary>
    public Rigidbody rBody;

    /// <summary>
    /// Starts this instance. Finds the rigidbody attached.
    /// </summary>
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Called once a frame. When the ball has hit the shield, the highest point of the ball will be taken before releasing an elemental. 
    /// This object is then destroyed.
    /// </summary>
    void Update() {

        if (shieldcontact)
        {

        }


        if (thrown)
        {
            if (!set)
            {
                yAxis = transform.position.y;
 
                set = true;
            }

            if (yAxis <= transform.position.y)
            {
                yAxis = transform.position.y;
            }
            else
            {
                

                if (!random)
                {
                    elemental[elementalID].SetActive(true);
                }
                else
                {
                    int chosenOne = Random.Range(0, elemental.Length);

                    foreach(GameObject g in elemental)
                    {
                        if (elemental[chosenOne] == g)
                        {
                            g.SetActive(true);
                        }
                        else
                        {
                            Destroy(g);
                        }
                    }

                    
                }

                sphere.SetActive(false);

                transform.DetachChildren();
                Destroy(this.gameObject);
                
            }
        }
        if (objFloat)
        {
            transform.position += transform.forward * Time.deltaTime * 1.5f;

            if (transform.root.tag == "Player")
            {
                objFloat = false;
            }
        }
    }

    /// <summary>
    /// Sets up this ball to be shot at player.
    /// </summary>
    public void floating()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = false;
        transform.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        transform.localScale = new Vector3(2, 2, 2);
        objFloat = true;
    }
    /// <summary>
    /// If the ball has been thrown.
    /// </summary>
    public void threw(){

        thrown = true;
        }


    /// <summary>
    /// If contact with the shield has been made.
    /// </summary>
    public void ShieldTouch()
    {
        GetComponent<Rigidbody>().useGravity = true;
        shieldcontact = true;
        objFloat = false;

        thrown = true;
    }

    /// <summary>
    /// If contact with the shield has been made.
    /// </summary>
    /// <param name="x">The x.</param>
    /// <param name="c">The c.</param>
    /// <param name="trans">The transform.</param>
    public void ShieldTouch(float x, Collision c, Transform trans)
    {
        GetComponent<Rigidbody>().useGravity = true;
        shieldcontact = true;
        objFloat = false;


    }
}
