// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="Level.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Level.
/// </summary>
public class Level {

    /// <summary>
    /// The levelnum
    /// </summary>
    public int levelnum;
    /// <summary>
    /// The waves
    /// </summary>
    public List<Wave> waves;

    /// <summary>
    /// Initializes a new instance of the <see cref=".Level"/> class.
    /// </summary>
    /// <param name="levelnum">The levelnum.</param>
    /// <param name="waves">The waves.</param>
    public Level(int levelnum, List<Wave> waves)
    {
        this.levelnum = levelnum;
        this.waves = waves;
    }

}
