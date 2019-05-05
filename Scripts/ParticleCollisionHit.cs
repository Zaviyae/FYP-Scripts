// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="ParticleCollisionHit.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class ParticleCollisionHit. Helper script for reporting a hit from a particle effect.
/// </summary>
public class ParticleCollisionHit : MonoBehaviour {



    /// <summary>
    /// Called when [particle collision].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnParticleCollision(GameObject other)
    {

         GetComponentInParent<TargetBlast>().Hit();


    }
}
