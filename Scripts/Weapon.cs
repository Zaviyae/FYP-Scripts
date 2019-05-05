// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="Weapon.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class Weapon.
/// </summary>
public class Weapon : MonoBehaviour {

    /// <summary>
    /// The Gameobject which is used for position to emit particles from.
    /// </summary>
    public GameObject particleEmit;
    /// <summary>
    /// The player
    /// </summary>
    public Player player;
    /// <summary>
    /// If the beam should currently be active.
    /// </summary>
    public bool beamActive;
    /// <summary>
    /// These are the instances of the start, beginning and end prefab for the currently chosen ElementType mode.
    /// </summary>
    private GameObject ibeamStart, ibeam, ibeamEnd;
    /// <summary>
    /// The line renderer used for instantiating the beam.
    /// </summary>
    private LineRenderer line;
    /// <summary>
    /// The current element type.
    /// </summary>
    private ElementType.Type type;
    /// <summary>
    /// The current enemy the beam is hitting
    /// </summary>
    public Enemy currentEnemy;
    /// <summary>
    /// How long the beam can be active.
    /// </summary>
    public float beamSeconds = 0f;
    /// <summary>
    /// The text element used to display the cast score.
    /// </summary>
    public Text scoreText;
    /// <summary>
    /// Is the beam currently showing
    /// </summary>
    public bool beaming;
    /// <summary>
    /// The text element showing the beam seconds to the player
    /// </summary>
    public Text beamCount;
    /// <summary>
    /// The wand mesh
    /// </summary>
    public MeshRenderer wandMesh;
    /// <summary>
    /// The red, purple and blue materials to be used for the wand mesh.
    /// </summary>
    public Material red, purple, blue;
    /// <summary>
    /// The red, purple and blue meshes to be used for the wand object.
    /// </summary>
    public GameObject redMesh, purpleMesh, blueMesh;
    /// <summary>
    /// The current ElementType.Type as a string.
    /// </summary>
    string currentschool;
    /// <summary>
    /// The beam end offset
    /// </summary>
    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    /// <summary>
    /// The texture scroll speed
    /// </summary>
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    /// <summary>
    /// The texture length scale
    /// </summary>
    public float textureLengthScale = 3; //Length of the beam texture

    public Text modeText;
    public GameObject particleComplete;

    /// <summary>
    /// Each individual start, end and middle beam prefab for every element type.
    /// </summary>
    public GameObject beamStartb, beamb, beamEndb,

            beamStartr, beamr, beamEndr,

            beamStartp, beamp, beamEndp; 

    /// <summary>
    /// Returns the start, middle and end object for the appropriate ElementType.Type.
    /// </summary>
    /// <param name="colour">The colour.</param>
    /// <returns>List&lt;GameObject&gt;.</returns>
    public List<GameObject> returnBeams(string colour)
    {
        List<GameObject> returnedBeams = new List<GameObject>();
        switch (colour)
        {
            case "Blue":
                returnedBeams.Add(beamStartb);
                returnedBeams.Add(beamb);
                returnedBeams.Add(beamEndb);
                type = ElementType.Type.Blue;
                currentschool = "Blue";
                break;
            case "Red":
                returnedBeams.Add(beamStartr);
                returnedBeams.Add(beamr);
                returnedBeams.Add(beamEndr);
                type = ElementType.Type.Red;
                currentschool = "Red";
                break;

            case "Purple":
                returnedBeams.Add(beamStartp);
                returnedBeams.Add(beamp);
                returnedBeams.Add(beamEndp);
                type = ElementType.Type.Purple;
                currentschool = "Purple";
                break;

            default:
                print("default");
                break;

        }
        return returnedBeams;

    }

    /// <summary>
    /// Updates the mesh. Used for resetting meshes after a cast.
    /// </summary>
    /// <param name="el">The el.</param>
    /// <param name="y">if set to <c>true</c> [y].</param>
    public void UpdateMesh(ElementType.Type el, bool y)
    {
            switch (el)
            {
                case ElementType.Type.Blue:
                    transform.GetComponentInChildren<MeshRenderer>().material = blue;
                    transform.GetComponentInChildren<MeshRenderer>().materials[0] = blue;
                    ChangeMesh("Blue");
                if (y)
                {
                    blueMesh.SetActive(true);
                    blueMesh.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(wandMesh.gameObject);
                }
                    break;
                case ElementType.Type.Red:
                    transform.GetComponentInChildren<MeshRenderer>().material = red;
                    transform.GetComponentInChildren<MeshRenderer>().materials[0] = red;
                    ChangeMesh("Red");
                if (y)
                {
                    redMesh.SetActive(true);
                    redMesh.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(wandMesh.gameObject);
                }
                    break;
                case ElementType.Type.Purple:
                    transform.GetComponentInChildren<MeshRenderer>().material = purple;
                    transform.GetComponentInChildren<MeshRenderer>().materials[0] = purple;
                    ChangeMesh("Purple");
                if (y)
                {
                    purpleMesh.SetActive(true);
                    purpleMesh.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect(wandMesh.gameObject);
                }
                    break;
            }
        
        if(!y)
        {
            blueMesh.SetActive(false);
            redMesh.SetActive(false);
            purpleMesh.SetActive(false);
        }
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(damageTick());
        beamCount.text = beamSeconds.ToString("F0");
        player.AssignElement();

    }

    /// <summary>
    /// Changes the mesh. Changing the colour of the wand to the appropriate ElementType.Type.
    /// </summary>
    /// <param name="s">The s.</param>
    public void ChangeMesh(string s)
    {
        switch (s)
        {
            case "Red":
                wandMesh.material = red;
                break;
            case "Blue":
                wandMesh.material = blue;
                break;
            case "Purple":
                wandMesh.material = purple;
                break;
        }
    }
    /// <summary>
    /// Beams the specified ElementType.Type.
    /// </summary>
    /// <param name="school">The school.</param>
    public void Beam(string school)
    {
        
        //Red Purple or Blue
        print("start beam");

        if (ibeam)
        {
            print("clear beams");
            Destroy(ibeam);
            Destroy(ibeamStart);
            Destroy(ibeamEnd);

        }
  
        GameObject[] converted = returnBeams(school).ToArray();

        ibeamStart = Instantiate(converted[0], transform.parent.position, transform.parent.rotation);
        ibeam = Instantiate(converted[1], transform.parent.position, transform.parent.rotation);
        ibeamEnd = Instantiate(converted[2], transform.parent.position, transform.parent.rotation);


        line = ibeam.GetComponent<LineRenderer>();
        beamActive = true;

    }

    /// <summary>
    /// Ends the beam.
    /// </summary>
    public void EndBeam()
    {

        beamActive = false;
    }

    /// <summary>
    /// Shoots the beam in direction from start.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="dir">The direction.</param>
    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        ibeamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit))
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = transform.position + (dir * 100);

        
        
        if (hit.transform.tag == "Enemy")
        {
            currentEnemy = hit.transform.gameObject.GetComponent<Enemy>();
        }
        else
        {
            currentEnemy = null;
        }

        if(hit.transform.tag == "Projectile")
        {
            Destroy(hit.transform.gameObject);
        }


        ibeamEnd.transform.position = end;
        line.SetPosition(1, end);

        ibeamStart.transform.LookAt(ibeamEnd.transform.position);
        ibeamEnd.transform.LookAt(ibeamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }

    /// <summary>
    /// Applies damage to the current Enemy every .25f seconds.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator damageTick()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.25f);
            if (player) { scoreText.text = ((player.currentScore - 5) * 10).ToString() + " %";  }

            if (currentEnemy)
            {
                
                currentEnemy.TakeDamage(player.calcDamage(currentschool+"Beam"), type);
                currentEnemy = null;
            }
        }
    }


    /// <summary>
    /// Ran once per frame.
    /// Used for counting down beam seconds, enabling and disabling beam and updating the wand UI.
    /// </summary>
    private void Update()
    {
        if (beaming)
        {
            beamCount.enabled = true;

        }
        else
        {
            beamCount.enabled = false;
        }

        if (beamActive)
        {
            beamSeconds -= 1 * Time.deltaTime;
            beamCount.text = beamSeconds.ToString("F0");
            if(beamSeconds <= 0)
            {
                player.BeamTimed();
                beamActive = false;
            }


            RaycastHit hit;
            if (Physics.Raycast(transform.parent.position, transform.parent.TransformDirection(Vector3.forward), out hit))  //ray origin, ray direction
            {
                Vector3 tdir = hit.point - transform.position;
                ShootBeamInDir(particleEmit.transform.position, tdir);
            }
            else
            {
                ShootBeamInDir(particleEmit.transform.position, transform.forward);
            }
        }
        else
        {
            if (ibeam)
            {
                print("beam destroy!");
                Destroy(ibeam);
                Destroy(ibeamStart);
                Destroy(ibeamEnd);
        
            }
        }

    }
}
