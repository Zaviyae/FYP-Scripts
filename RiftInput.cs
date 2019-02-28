﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Linq;
using System.IO;
using System;

public class RiftInput : MonoBehaviour {

 
    public SteamVR_Action_Single squeezeAction;

    public SteamVR_Action_Vector2 touchPadAction;



    public GameObject drawingParticles;

    public bool[,] vectorGrid;
    public HashSet<Vector2> rawVectors = new HashSet<Vector2>();

    public int count = 1;
    public DollarRecognizer dollarR;

    public TextAsset[] Saved;

    public Valve.VR.InteractionSystem.Player vrPlayer;

    public Weapon weapon;

    public Player player;

    public Valve.VR.InteractionSystem.Hand hand;

    public bool drawing, switching, equipped = false;
    
    void Start () {

        try
        {

            if (Saved != null)
            {
                //print("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<SAVE PATTERNS>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                print(Saved.Count());
                foreach (TextAsset t in Saved)
                {
                    print(t.name);
                    List<Vector2> parsedVectors = new List<Vector2>();
                    string parsedstring = t.text.Substring(0, t.text.Length - 1);
                    string[] stringvectors = parsedstring.Split('|');
                    foreach (string s in stringvectors)
                    {

                        string tmp = s;
                        tmp.Trim();
                        // //print(s.Length -1);
                        tmp = s.Substring(1, s.Length - 2);

                        string[] sparts = tmp.Split(',');
                        float px = System.Convert.ToSingle(sparts[0]);
                        float py = System.Convert.ToSingle(sparts[1]);
                        Vector2 parsedv = new Vector2(px, py);
                        parsedVectors.Add(parsedv);

                    }
                    // savedVectorlists[count - 1] = parsedVectors;
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

    private void FixedUpdate()
    {

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

                // Track(new Vector2(drawingParticles.transform.position.x, drawingParticles.transform.position.y));
                Track(new Vector2(hand.transform.localPosition.x, hand.transform.localPosition.y));
               // print("World space : " + hand.transform.position );
               // print("Local space : " + hand.transform.localPosition);

                drawingParticles = vrPlayer.rightHand.GetComponentInChildren<Weapon>().particleEmit;
                
            }

            if (SteamVR_Input._default.inActions.LeftGrip.GetState(SteamVR_Input_Sources.LeftHand))
            {
                switching = true;
                if (SteamVR_Input._default.inActions.RightGrip.GetLastStateDown(SteamVR_Input_Sources.RightHand))
                {
                    player.IncrementSkillMode();

                }
            }
            else
            {
                switching = false;
            }

        }
    }

    void Update () {

        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
        {
            player.Teleport();
        }



        float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if (triggerValue > 0.01f)
        {
           // //print(triggerValue);
        }

        Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if(touchpadValue != Vector2.zero)
        {
            ////print(touchpadValue);
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

    void Track(Vector2 pos)
    {
        rawVectors.Add(pos * 1000);
       // rawVectors.Add(pos*10000);
    }

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

    void OutputList(TextAsset t)
    {
        List<float> xValues = new List<float>();
        List<float> yValues = new List<float>();

        string[] stringvectors = t.text.Split('|');

        foreach (string s in stringvectors)
        {

            string tmp = s;
            tmp.Trim();
            // //print(s.Length -1);
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