// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="Projectile.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Projectile.
/// Attached to any projectile in the game.
/// </summary>
public class Projectile : MonoBehaviour {
    /// <summary>
    /// The rigidbody attached to the projectile.
    /// </summary>
    Rigidbody rbody;


    /// <summary>
    /// The explosion effect to spawn on impact.
    /// </summary>
    public GameObject explosionEffect;

    /// <summary>
    /// The element type of this projectile.
    /// </summary>
    public ElementType.Type elementType;

    /// <summary>
    /// The damage to apply.
    /// </summary>
    public int damage = 10;

    /// <summary>
    /// The speed of the projectile.
    /// </summary>
    public float speed;
    /// <summary>
    /// The player object
    /// </summary>
    public GameObject player;

    /// <summary>
    /// If slow motion is triggered.
    /// </summary>
    bool triggered;

    /// <summary>
    /// The RFX (third party) script reference
    /// </summary>
    private RFX4_EffectSettings rfx;


    /// <summary>
    /// Starts this instance. Finds the player in the scene.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        try
        {
            var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
            if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

            var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
            if (raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
            
            rfx = GetComponent<RFX4_EffectSettings>();
        }
        catch (Exception e)
        {

        }


    }

    /// <summary>
    /// Called once per frame, checks if the projectile is within certain distance from the player to apply slow motion.
    /// </summary>
    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            SlowMo();
        }
    }

    /// <summary>
    /// Applies slow motion, reducing speed of projectile to 20% of base.
    /// </summary>
    private void SlowMo()
    {
        if (!triggered)
        {
            triggered = true;
            if(rbody) rbody.velocity = (rbody.velocity * 0.2f);

            if (rfx)
            {
                rfx.Speed = (rfx.Speed * 0.5f);
            }
        }

    }


    /// <summary>
    /// Called when [trigger enter]. Checks if projectile has hit the players shield.
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
           
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// Overrides the current element type.
    /// </summary>
    /// <param name="elType">Type of the el.</param>
    public void Override(ElementType.Type elType)
    {
        elementType = elType;

    }

    /// <summary>
    /// Called when [collision enter]. Checks if projectile hit the player.
    /// </summary>
    /// <param name="collision">The collision.</param>
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.tag == "Player")
        {
            print("hit player!");
        }


    }
    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    { }

    }
