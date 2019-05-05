// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="RiftInput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Linq;
using System.IO;
using System;

/// <summary>
/// Class RiftInput.
/// Used for handling all input from the rift controllers.
/// </summary>
public class RiftInput : MonoBehaviour {

    /// <summary>
    /// The shield and wand pickup objects
    /// </summary>
    public GameObject shieldPickup, wandPickup;
    /// <summary>
    /// The rift input squeeze action
    /// </summary>
    public SteamVR_Action_Single squeezeAction;

    /// <summary>
    /// The touch pad action
    /// </summary>
    public SteamVR_Action_Vector2 touchPadAction;



    /// <summary>
    /// The drawing particles
    /// </summary>
    public GameObject drawingParticles;


    /// <summary>
    /// The raw vectors for gesture recognition
    /// </summary>
    public HashSet<Vector2> rawVectors = new HashSet<Vector2>();

    /// <summary>
    /// The count. Used for counting.
    /// </summary>
    public int count = 1;

    /// <summary>
    /// The dollar recogniser instance.
    /// </summary>
    public DollarRecognizer dollarR;

    /// <summary>
    /// The saved text asset array. To be converted to saved gestures.
    /// </summary>
    public TextAsset[] Saved;

    /// <summary>
    /// The vr player
    /// </summary>
    public Valve.VR.InteractionSystem.Player vrPlayer;

    /// <summary>
    /// Reference to the weapon
    /// </summary>
    public Weapon weapon;
    /// <summary>
    /// The shield game object.
    /// </summary>
    public GameObject shield;

    /// <summary>
    /// The player reference.
    /// </summary>
    public Player player;

    /// <summary>
    /// The right hand reference.
    /// </summary>
    public Valve.VR.InteractionSystem.Hand hand;
    /// <summary>
    /// The left hand reference.
    /// </summary>
    public Valve.VR.InteractionSystem.Hand leftHand;

    /// <summary>
    /// Bools for if the player is currently drawing, currently switching element and if currently equipped wand
    /// </summary>
    public bool drawing, switching, equipped = false;

    /// <summary>
    /// Starts this instance.
    /// Sets up parsing saved gestures from saved texts.
    /// </summary>
    void Start () {
        shield = null;
        try
        {

            if (Saved != null)
            {
                //print("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<SAVE PATTERNS>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                print(Saved.Count());
                foreach (TextAsset t in Saved)
                {
                   
                    List<Vector2> parsedVectors = new List<Vector2>();
                    string parsedstring = t.text.Substring(0, t.text.Length - 1);
                    string[] stringvectors = parsedstring.Split('|');
                    foreach (string s in stringvectors)
                    {

                        string tmp = s;
                        tmp.Trim();
                        
                        tmp = s.Substring(1, s.Length - 2);

                        string[] sparts = tmp.Split(',');
                        float px = System.Convert.ToSingle(sparts[0]);
                        float py = System.Convert.ToSingle(sparts[1]);
                        Vector2 parsedv = new Vector2(px, py);
                        parsedVectors.Add(parsedv);

                    }
               
                    foreach (Vector2 v in parsedVectors)
                    {

                    }
                    dollarR.SavePattern(t.name, parsedVectors.ToArray());

                }


                string[] l = dollarR.EnumerateGestures();


            }
        }catch(Exception e)
        {

        }

    }

    /// <summary>
    /// Called once a frame. 
    /// Used for checking all inputs to the rift controller and what to do.
    /// </summary>
    private void FixedUpdate()
    {
   

        if (shield != null) shieldPickup.transform.position = new Vector3(100, 100, 100);
        if (weapon != null) wandPickup.transform.position = new Vector3(100, 100, 100);

        if (shield == null && leftHand.currentAttachedObject && leftHand.currentAttachedObject.tag == "Shield")
        {
            shield = leftHand.currentAttachedObject;
        }
        if (equipped == false && hand.currentAttachedObject && hand.currentAttachedObject.GetComponentInChildren<Weapon>())
        {
            equipped = true;
            weapon = hand.currentAttachedObject.GetComponentInChildren<Weapon>();
            player.wandMeshEffects = weapon.transform.GetComponentsInChildren<PSMeshRendererUpdater>();

            foreach (PSMeshRendererUpdater g in player.wandMeshEffects)
            {
                g.transform.gameObject.SetActive(false);
            }
           
            for(int i = 0; i < weapon.GetComponentInChildren<MeshRenderer>().materials.Length; i++)
            {
                weapon.GetComponentInChildren<MeshRenderer>().materials[i] = player.baseMat;
            }

        }
        if (equipped)
        {
            if (SteamVR_Input._default.inActions.ButtonA.GetLastStateDown(SteamVR_Input_Sources.RightHand))
            {
                print("shields up!");
                player.ShieldUp(true);
            }

            if (SteamVR_Input._default.inActions.ButtonA.GetLastStateUp(SteamVR_Input_Sources.RightHand))
            {
                print("shields down!");
                player.ShieldUp(false);
            }

            if (player.casting && SteamVR_Input._default.inActions.Fire.GetLastStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand)) return;

                player.Fire();
            }

            if (!player.casting && SteamVR_Input._default.inActions.Fire.GetLastStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand)) return;

                player.Fire(true);
            }

            if (player.casting && SteamVR_Input._default.inActions.Fire.GetLastStateUp(SteamVR_Input_Sources.RightHand))
            {
                
                if (SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand)) return;
                
                player.Beam(false);
            }

            if (!SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.Any) && !SteamVR_Input._default.inActions.Fire.GetState(SteamVR_Input_Sources.Any))
            {
                player.Beam(false);
            }

            if (!player.casting && SteamVR_Input._default.inActions.Fire.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand)) return;

                player.Beam(true);
            }

            if (SteamVR_Input._default.inActions.LeftGrip.GetState(SteamVR_Input_Sources.LeftHand))
            {
                player.ClearSpell();
                
            }

            if (SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.RightHand) && SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand))
            {
                drawing = true;
                player.ClearSpell();
            }


            if (drawing && SteamVR_Input._default.inActions.Draw.GetState(SteamVR_Input_Sources.LeftHand))
            {
                if (!drawingParticles)
                {
                    try
                    {
                        drawingParticles = vrPlayer.rightHand.GetComponentInChildren<Weapon>().particleEmit;
                        
     
                    }
                    catch (Exception e)
                    {

                    }
                }
                
                drawingParticles.SetActive(true);

 
                Track(new Vector2(hand.transform.localPosition.x, hand.transform.localPosition.y));


                drawingParticles = vrPlayer.rightHand.GetComponentInChildren<Weapon>().particleEmit;
                
            }

            if (SteamVR_Input._default.inActions.LeftGrip.GetState(SteamVR_Input_Sources.LeftHand))
            {
                switching = true;
                if (SteamVR_Input._default.inActions.RightGrip.GetLastStateDown(SteamVR_Input_Sources.RightHand))
                {
                    print("tried to increment");
                    player.IncrementSkillMode();

                }
            }
            else
            {
                switching = false;
            }

        }
    }

    /// <summary>
    /// Called once a frame.
    /// Used for getting input and what to do with it.
    /// </summary>
    void Update () {

        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            player.Teleport();
        }



        float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if (triggerValue > 0.01f)
        {
         
        }

        Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if(touchpadValue != Vector2.zero)
        {
   
        }
        if (equipped)
        {
            if (SteamVR_Input._default.inActions.Draw.GetLastStateUp(SteamVR_Input_Sources.RightHand) && drawing)
            {
                Check();
                if (drawingParticles)
                {
                    drawingParticles.SetActive(false);
                }
                drawing = false;
            }
        }
    }

    /// <summary>
    /// Tracks the vector positions passed to the raw vector array.
    /// </summary>
    /// <param name="pos">The vector position.</param>
    void Track(Vector2 pos)
    {
        rawVectors.Add(pos * 1000);

    }

    /// <summary>
    /// Checks this the vectors, and will fail the cast if the vector count is too small.
    /// </summary>
    void Check()
    {

        
        
       SaveList();

        print(rawVectors.Count);


        if (rawVectors.Count >= 40)
        {
            player.CastSpell(dollarR.Recognize(rawVectors.ToArray()));
        }
        else
        {
            player.CastSpell();
        }
        



        rawVectors = new HashSet<Vector2>();
        
    }

    /// <summary>
    /// Saves the list of raw vectors to specified position.
    /// </summary>
    void SaveList()
    {
        string saveString = "";
        foreach (Vector2 v in rawVectors)
        {
            saveString += v.ToString() + "|";
        }

        print(saveString);
        System.IO.File.WriteAllText("Assets/Resources/test" + count + ".txt", saveString);
    
        count++;
    }

    /// <summary>
    /// Outputs the list of raw vectors.
    /// </summary>
    /// <param name="t">The t.</param>
    void OutputList(TextAsset t)
    {
        List<float> xValues = new List<float>();
        List<float> yValues = new List<float>();

        string[] stringvectors = t.text.Split('|');

        foreach (string s in stringvectors)
        {

            string tmp = s;
            tmp.Trim();
            tmp = s.Substring(1, s.Length - 2);

            string[] sparts = tmp.Split(',');
            float px = System.Convert.ToSingle(sparts[0]);
            float py = System.Convert.ToSingle(sparts[1]);

            xValues.Add(px);
            yValues.Add(py);

        }

        string saveString = "";
        foreach (float v in xValues)
        {
            saveString += v.ToString() + ",";
        }

        //print(saveString);
       // System.IO.File.WriteAllText("Assets/Resources/" + t.name + "XVALUES.txt", saveString);

        saveString = "";
        foreach (float v in yValues)
        {
            saveString += v.ToString() + ",";
        }

        //print(saveString);
        //System.IO.File.WriteAllText("Assets/Resources/" + t.name + "YVALUES.txt", saveString);
    }
}
