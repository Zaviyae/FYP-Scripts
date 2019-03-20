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

    public bool beamActive;

    public GameObject beamStart, beam, beamEnd;
    public GameObject ibeamStart, ibeam, ibeamEnd;
    private LineRenderer line;

    public Enemy currentEnemy;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture


    private void Start()
    {
        StartCoroutine(damageTick());
    }

    public void Beam()
    {
        print("start beam");

        if (ibeam)
        {
            print("clear beams");
            Destroy(ibeam);
            Destroy(ibeamStart);
            Destroy(ibeamEnd);

        }

        ibeamStart = Instantiate(beamStart, transform.parent.position, transform.parent.rotation);
        ibeam = Instantiate(beam, transform.parent.position, transform.parent.rotation);
        ibeamEnd = Instantiate(beamEnd, transform.parent.position, transform.parent.rotation);


        line = ibeam.GetComponent<LineRenderer>();
        beamActive = true;

    }

    public void EndBeam()
    {
        print("end beam");
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
            if (currentEnemy)
            {
                currentEnemy.TakeDamage(6);
                currentEnemy = null;
            }
        }
    }


    private void Update()
    {
        if (beamActive)
        {

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
