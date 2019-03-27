using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

    public GameObject particleEmit;
    public GameObject particleComplete;
    public Text modeText;
    public GameObject enemyTarget;
    public GameObject shield;
    public Player player;
    public bool beamActive;

    private GameObject ibeamStart, ibeam, ibeamEnd;
    private LineRenderer line;
    private ElementType.Type type;

    public Enemy currentEnemy;

    public float beamSeconds = 0f;

    public Text scoreText;

    public bool beaming;

    public Text beamCount;

    public MeshRenderer wandMesh;
    public Material red, purple, blue;


    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture



    public GameObject beamStartb, beamb, beamEndb,
            beamStartr, beamr, beamEndr,
            beamStartp, beamp, beamEndp; //blue red and purple. The schools.

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
                break;
            case "Red":
                returnedBeams.Add(beamStartr);
                returnedBeams.Add(beamr);
                returnedBeams.Add(beamEndr);
                type = ElementType.Type.Red;
                break;

            case "Purple":
                returnedBeams.Add(beamStartp);
                returnedBeams.Add(beamp);
                returnedBeams.Add(beamEndp);
                type = ElementType.Type.Purple;
                break;

            default:
                print("default");
                break;

        }
        return returnedBeams;

    }

    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(damageTick());
        beamCount.text = beamSeconds.ToString("F0");
        player.AssignElement();

    }

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

    public void EndBeam()
    {

        beamActive = false;
    }

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

    IEnumerator damageTick()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.25f);
            if (player) { scoreText.text = ((player.currentScore - 5) * 10).ToString() + " %";  }

            if (currentEnemy)
            {
                
                currentEnemy.TakeDamage(player.calcDamage(1), type);
                currentEnemy = null;
            }
        }
    }


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

        /*
        RaycastHit hit;

        if (Physics.Raycast(particleComplete.transform.position, particleComplete.transform.TransformDirection(Vector3.forward), out hit, 250f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            if (enemyTarget)
            {
                enemyTarget.GetComponent<Enemy>().targetted = false;
                enemyTarget = null;
            }
            
            if(hit.transform.tag == "Enemy")
            {
                enemyTarget = hit.transform.gameObject;
                enemyTarget.GetComponent<Enemy>().targetted = true;
            }
        }
        */
    }
}
