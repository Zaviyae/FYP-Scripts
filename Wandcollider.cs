using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandcollider : MonoBehaviour {

    List<GameObject> enemies;
    GameObject temptarget, target;
    public Player player;
	// Use this for initialization
	void Start () {
        enemies = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (target)
        {
            if (!target.GetComponent<Enemy>().isActiveAndEnabled)
            {
                enemies.Remove(target);
            }
        }


        float smallestdist = Mathf.Infinity;
        if (enemies.Count == 0)
            return;
		foreach(GameObject o in enemies)
        {
            if(Vector3.Distance(transform.position, o.transform.position) < smallestdist)
            {
                smallestdist = Vector3.Distance(transform.position, o.transform.position);

                if (o.GetComponent<Enemy>())
                {
                    temptarget = o.gameObject;
                }
                else
                {
                    try
                    {
                        temptarget = o.transform.root.gameObject;
                    }catch(Exception e)
                    {

                    }
                }
                
                
            }
        }
        if (target == null)
        {
            target = temptarget;
            target.GetComponent<Enemy>().targetted = true;
        }
        else
        {
            target.GetComponent<Enemy>().targetted = false;
            target = temptarget;
            target.GetComponent<Enemy>().targetted = true;
        }
        player.target = target;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject);
        }
        else
        {
            if (collision.transform.root.tag == "Enemy")
            {
                enemies.Add(collision.transform.root.gameObject);
            }
            else
            {
                if (target)
                {
                    target.GetComponent<Enemy>().targetted = false;
                    target = null;
                }
            }

        }
    }

    private void OnTriggerExit(Collider collision)
    {


        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Remove(collision.gameObject);
        }
        else
        {
            if (collision.transform.root.tag == "Enemy")
            {
                enemies.Remove(collision.transform.root.gameObject);
            }
            else
            {
                if (target && enemies.Count == 0)
                {
                    target.GetComponent<Enemy>().targetted = false;
                    target = null;
                }
            }

        }
    }
}
