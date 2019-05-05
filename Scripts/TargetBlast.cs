// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="TargetBlast.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class TargetBlast. Used on each blast ability available in the game.
/// </summary>
public class TargetBlast : MonoBehaviour {

    /// <summary>
    /// Is the blast player controlled
    /// </summary>
    public bool playerControlled;

    /// <summary>
    /// The target to hit
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The damage to apply.
    /// </summary>
    public int damage;
    /// <summary>
    /// The ElementType of the hit.
    /// </summary>
    public ElementType.Type type;
    /// <summary>
    /// The y increase to the positioning of the blast object if necessary.
    /// </summary>
    public float yIncrease;
    /// <summary>
    /// seconds : The seconds the effect of this blast will last. timetilldamage : The seconds until damage is applied.
    /// </summary>
    public float seconds, timetilldamage;
    /// <summary>
    /// instantDamage : Does this effect apply damage instantly? collisionDamage : Does this effect apply damage on contact with the particle effect?
    /// </summary>
    public bool instantDamage, collisionDamage;
    /// <summary>
    /// Has the raycast hit.
    /// </summary>
    bool hit;
    /// <summary>
    /// The Enemy script attached to the target.
    /// </summary>
    Enemy enemy;
    /// <summary>
    /// The player
    /// </summary>
    Player player;
    /// <summary>
    /// The RFX third party script used for effects.
    /// </summary>
    RFX1_TransformMotion rfx;
    /// <summary>
    /// If this blast uses RFX
    /// </summary>
    public bool useRFX;
    /// <summary>
    /// The skill name
    /// </summary>
    public string skillName;

    /// <summary>
    /// Enum blastType. The types of effects this target blast can be.
    /// </summary>
    public enum blastType
    {
        ROOT, FREEZE, DAMAGE, SUSPEND
    }


    /// <summary>
    /// What blastType this blast is.
    /// </summary>
    public blastType myType;
    /// <summary>
    /// Variables for determining how this blast particle effect is instantiated.
    /// </summary>
    public bool useCustomLocation, parentcustomlocation, useCustomObject;
    /// <summary>
    /// If this blast effect uses a custom location id/ custom object id.
    /// </summary>
    public int customLocationID, customObjectID;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        if (useRFX) { rfx = GetComponentInChildren<RFX1_TransformMotion>(); rfx.targetBlast = this; }
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

        var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if (raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
        SetUp();
        }

    /// <summary>
    /// Sets up the blast, instantiating all required particle effects and positioning.
    /// </summary>
    public void SetUp()
    {
        hit = false;
        enemy = target.GetComponent<Enemy>();
        if (!useCustomObject)
        {
            if (!useCustomLocation)
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + yIncrease, target.transform.position.z);
            }
            else
            {
                transform.position = enemy.blastPoints[customLocationID].transform.position;
                transform.rotation = enemy.blastPoints[customLocationID].transform.rotation;
                if (parentcustomlocation)
                {
                    transform.parent = enemy.blastPoints[customLocationID];
                }
            }
        }
        else
        {

            CustomObjectSpawn(customObjectID);

        }

        if (myType == blastType.ROOT)
        {
            // transform.parent = target.transform;
            enemy.Root(seconds);
            if (instantDamage)
            {
                enemy.TakeDamage(player.calcDamage(skillName), type);
            }
        }

        if (myType == blastType.DAMAGE)
        {
            //transform.parent = target.transform;
            enemy.Root(seconds);
            if (instantDamage)
            {
                StartCoroutine(Damage(timetilldamage));
            }

        }

        if (myType == blastType.SUSPEND)
        {
            enemy.Airbound(5);

        }

        if (myType == blastType.FREEZE)
        {
            enemy.Freeze(seconds);
            if (instantDamage)
            {
                enemy.TakeDamage(player.calcDamage(skillName), type);
            }
            else
            {
                if (!collisionDamage)
                {
                    StartCoroutine(Damage(timetilldamage));
                }
                
            }
        }

        
    }

    /// <summary>
    /// For custom particle effect object instantiating.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public void CustomObjectSpawn(int id)
    {
        print(id);
        enemy.blastObjects[id].SetActive(false);
        enemy.blastObjects[id].SetActive(true);

    }


    /// <summary>
    /// When a collision with particle effects has been found. 
    /// Check if it is an enemy then apply damage.
    /// </summary>
    /// <param name="sender">The collision sender.</param>
    /// <param name="e">The RFX script e.</param>
    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        if (!hit && collisionDamage)
        {
            hit = true;
            if (e.HitGameObject.tag == "Enemy")
            {
                Enemy enemy = e.HitGameObject.transform.GetComponent<Enemy>();

                if (enemy)
                {
                    StartCoroutine(Damage(timetilldamage));

                }
            }
            else
            {
                if (e.HitGameObject.transform.root.tag == "Enemy")
                {
                    Enemy enemy = e.HitGameObject.transform.root.GetComponent<Enemy>();

                    if (enemy)
                    {
                        StartCoroutine(Damage(timetilldamage));
                    }
                }
            }

            Debug.Log("hit - " + name);
        }
        
    }

    /// <summary>
    /// If this blast has hit the enemy.
    /// </summary>
    public void Hit()
    {
        
        print("Target hit");
        StartCoroutine(Damage(timetilldamage));
       

    }

    /// <summary>
    /// Damages after the specified time.
    /// </summary>
    /// <param name="time">The time.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator Damage(float time)
    {
        yield return new WaitForSeconds(time);
        enemy.anim.speed = 1f;

        if (playerControlled)
        {
            enemy.TakeDamage(player.calcDamage(skillName), type);
        }
        else
        {
            enemy.TakeDamage(damage, type);
        }
        
    }
}
