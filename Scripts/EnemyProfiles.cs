// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-31-2019
// ***********************************************************************
// <copyright file="EnemyProfiles.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class EnemyProfiles. Used for enemy types.
/// </summary>
public class EnemyProfiles {

    /// <summary>
    /// The maximum health
    /// </summary>
    public int maxHealth = 40;
    /// <summary>
    /// The maximum damage
    /// </summary>
    public int maxDamage = 10;

    /// <summary>
    /// The base distance
    /// </summary>
    public float baseDistance = 20;
    /// <summary>
    /// The base speed
    /// </summary>
    public float baseSpeed = 2f;
    /// <summary>
    /// The base size
    /// </summary>
    public float baseSize = 1f;
    /// <summary>
    /// The level
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// The random distance
    /// </summary>
    public float randomDistance;

    /// <summary>
    /// The attack1 animation string
    /// </summary>
    public string attack1 = "AttackTrigger";

    /// <summary>
    /// The class choies.
    /// </summary>
    public enum Class
    {
 
        Archer, Mage, Healer
    }


    /// <summary>
    /// The class of this profile.
    /// </summary>
    public Class myClass;


    /// <summary>
    /// Class Healer.
    /// </summary>
    public class Healer : EnemyProfiles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Healer"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public Healer(int level)
        {
            myClass = Class.Healer;
            Level = level;
            maxHealth = 40 * level;
            maxDamage = 1 * level;

            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }
        }
    }

    /// <summary>
    /// Class Archer.
    /// </summary>
    public class Archer : EnemyProfiles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Archer"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public Archer(int level)
        {
            attack1 = "Longbow";
            myClass = Class.Archer;
            Level = level;
            maxHealth = 40 * level;
            maxDamage *= level;


            randomDistance = Random.Range(baseDistance - 5, baseDistance + 10);
            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }
        }
        
    }



    /// <summary>
    /// Class Mage.
    /// </summary>
    public class Mage : EnemyProfiles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mage"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        public Mage(int level)
        {
            attack1 = "Cast1";
            myClass = Class.Mage;
            Level = level;
            maxHealth = 30 * level;

            randomDistance = Random.Range(baseDistance - 5, baseDistance + 10);

            switch (level)
            {
                case 1:
                    baseSize = .6f;
                    break;
                case 2:
                    baseSize = .7f;
                    break;
                case 3:
                    baseSize = .8f;
                    break;
                case 4:
                    baseSize = 1f;
                    break;
            }
        }
      
    }

}
