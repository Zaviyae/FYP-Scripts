// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="HitBox.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class HitBox. Non important script, used for applying damage from a specific skill.
/// </summary>
public class HitBox : MonoBehaviour {

    /// <summary>
    /// The effect collision box
    /// </summary>
    public bool effectCollisionBox;
    /// <summary>
    /// The t blast object
    /// </summary>
    public GameObject tBlastObject;
    /// <summary>
    /// The player
    /// </summary>
    public Player player;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (!effectCollisionBox)
            {
                GetComponentInParent<Player>().TakeDamage(10f);
                other.GetComponent<Enemy>().ReportDeath();
            }
            else
            {
                    float fDamage = tBlastObject.GetComponent<TargetBlast>().damage * ElementType.getDamageModifier(tBlastObject.GetComponent<TargetBlast>().type, other.GetComponent<Enemy>().elementType);
                    print("AOE damage : " + Mathf.RoundToInt(fDamage));


            }
        }
    }
}
