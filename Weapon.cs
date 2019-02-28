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

    private void Start()
    {
        
    }

    private void Update()
    {
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
