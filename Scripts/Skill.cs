// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-22-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-23-2019
// ***********************************************************************
// <copyright file="Skill.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Skill.
/// Used by elementals.
/// </summary>
[CreateAssetMenu(fileName = "Skill", menuName = "Create Skill", order = 1)]
public class Skill : ScriptableObject{

    /// <summary>
    /// The skill name
    /// </summary>
    public string skillName;
    /// <summary>
    /// The skill identifier
    /// </summary>
    public int skillID;
    /// <summary>
    /// If the skillID is used or not.
    /// </summary>
    public bool useSkillID = true;

    /// <summary>
    /// The type of blast effect.
    /// </summary>
    public enum blastType
    {
        ROOT, FREEZE, DAMAGE, SUSPEND
    }

    /// <summary>
    /// My blastType
    /// </summary>
    public blastType myType;
}
