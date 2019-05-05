// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="DisableAfter.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class DisableAfter. Helper script. Disables this object after certain time.
/// </summary>
public class DisableAfter : MonoBehaviour {

    /// <summary>
    /// The time
    /// </summary>
    public float time;


    /// <summary>
    /// Begins this instance.
    /// </summary>
    public void Begin()
    {
        StartCoroutine(deactivate());
    }

    /// <summary>
    /// Deactivates this instance.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator deactivate()
    {
        yield return new WaitForSeconds(time);
        transform.gameObject.SetActive(false);
    }

}
