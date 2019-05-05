// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 02-22-2019
//
// Last Modified By : zaviy
// Last Modified On : 02-22-2019
// ***********************************************************************
// <copyright file="CreateSkill.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class CreateSkill. Helper script used for making skills.
/// </summary>
public class CreateSkill {
    /// <summary>
    /// Creates this asset.
    /// </summary>
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        Skill asset = ScriptableObject.CreateInstance<Skill>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
