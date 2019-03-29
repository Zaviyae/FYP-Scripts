using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTrigger : MonoBehaviour {
    public bool flameTick;
    Enemy enemy;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
    
        if(other.tag == "Ball")
        {
            print("got ball! ");
            PowerUpThrow put = other.GetComponentInParent<PowerUpThrow>();
            put.ShieldTouch();
        }

    

    }


    private void OnTriggerExit(Collider other)
    {



    }

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
            print("got BALL!! " + other.relativeVelocity.magnitude);
            PowerUpThrow put = null;

            put = other.gameObject.GetComponent<PowerUpThrow>();
            print(put);
            print(other.relativeVelocity.magnitude);
            print(other);
            print(transform);
            put.ShieldTouch(other.relativeVelocity.magnitude, other, transform);
        }


    }


}
