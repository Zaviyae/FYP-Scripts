// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 03-29-2019
// ***********************************************************************
// <copyright file="GetTrigger.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class GetTrigger. Helper script to report when a trigger has made contact.
/// </summary>
public class GetTrigger : MonoBehaviour
{


    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Ball")
        {
            print("got ball! ");
            PowerUpThrow put = other.GetComponentInParent<PowerUpThrow>();
            put.ShieldTouch();
        }



    }



    /// <summary>
    /// Called when [collision enter].
    /// </summary>
    /// <param name="other">The other.</param>
    private void OnCollisionEnter(Collision other)
    {

        print(other.transform.tag);
        if (other.transform.tag == "Shield")
        {
            print("got shield! " + other.relativeVelocity.magnitude);
            PowerUpThrow put = GetComponent<PowerUpThrow>();
            put.ShieldTouch();
        }


        if (other.transform.tag == "Ball")
        {
            PowerUpThrow putt = null;

            putt = other.gameObject.GetComponent<PowerUpThrow>();
            print(putt);
            print(other.relativeVelocity.magnitude);
            print(other);
            print(transform);
            putt.ShieldTouch(other.relativeVelocity.magnitude, other, transform);
        }


    }
}



