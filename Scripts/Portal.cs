// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="Portal.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Portal.
/// Used for scene and level portals.
/// </summary>
public class Portal : MonoBehaviour
{
    /// <summary>
    /// A prefab for a next level portal
    /// </summary>
    public Portal nextLevelPortal;
    /// <summary>
    /// If this portal is a next level portal
    /// </summary>
    public bool levelPortal;
    /// <summary>
    /// The start size of the portal
    /// </summary>
    Vector3 startSize;
    /// <summary>
    /// The start rotation of the portal
    /// </summary>
    Quaternion startRotation;

    /// <summary>
    /// The position the player goes to.
    /// </summary>
    public Transform spawnPos;

    /// <summary>
    /// If this portal is enlarged
    /// </summary>
    private bool Enlarge;

    /// <summary>
    /// The vr cam
    /// </summary>
    public Camera VRCam;

    /// <summary>
    /// The child rotation transform, for rotating the player on arrival
    /// </summary>
    public Transform childRotationTransform;

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        childRotationTransform = GetComponentInChildren<RotateMe>().transform;
        startSize = childRotationTransform.localScale;
        startRotation = childRotationTransform.rotation;

        VRCam = Camera.main;

    }

    /// <summary>
    /// If this portal is looked at, then enlarge.
    /// </summary>
    public void LookedAt()
    {
        Enlarge = true;
    }

    /// <summary>
    /// This portal is no longer looked at, then return to base size.
    /// </summary>
    public void LookedAway()
    {
        Enlarge = false;
    }

    /// <summary>
    /// Called once a frame, handles sizing and rotation of portal.
    /// </summary>
    private void Update()
    {
        if (Enlarge && childRotationTransform.localScale.x < startSize.x * 2)
        {
            childRotationTransform.localScale = Vector3.Lerp(childRotationTransform.localScale, childRotationTransform.localScale * 2, 1.5f * Time.deltaTime);

            childRotationTransform.LookAt(VRCam.transform);


        }
        else
        {
            if (transform.localScale != startSize && !Enlarge)
            {
                childRotationTransform.localScale = Vector3.Lerp(childRotationTransform.localScale, startSize, 2f * Time.deltaTime);

                childRotationTransform.rotation = Quaternion.Lerp(childRotationTransform.rotation, startRotation, 1f * Time.deltaTime);

            }
        }
    }
}
