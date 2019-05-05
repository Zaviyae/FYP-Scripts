// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="PlayerView.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class PlayerView. 
/// For getting portals in the player's view.
/// </summary>
public class PlayerView : MonoBehaviour {

    /// <summary>
    /// The portal being looked at.
    /// </summary>
    public Portal lookingAt;

    /// <summary>
    /// The portals in the player view.
    /// </summary>
    public List<Portal> lookingAts;

    /// <summary>
    /// Called when [trigger enter]. Will get the portal and add to lookingAts list.
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();
        lookingAts.Add(other.transform.GetComponent<Portal>());

        lookingAt.LookedAt();
    }

    /// <summary>
    /// Called when [trigger exit]. Will remove portal from lookingAts list.
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();
        lookingAts.Remove(other.transform.GetComponent<Portal>());
        lookingAt.LookedAway();
        lookingAt = null;
    }

    /// <summary>
    /// Gets the first portal in the lookAts list.
    /// </summary>
    /// <returns>Portal.</returns>
    public Portal getPortal()
    {
        if (lookingAts.Count == 0) return null;

        return lookingAts[lookingAts.Count-1];
     
    }

}
