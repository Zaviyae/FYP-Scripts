// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="removeNoParent.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class removeNoParent.
/// Helper script for simply destroying the object this script is attached to if the parent is not active.
/// </summary>
public class removeNoParent : MonoBehaviour {


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update () {
        if (transform.parent == null) Destroy(this.gameObject);
	}
}
