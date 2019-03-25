using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour {

    public float time;
	// Use this for initialization
	void Start () {
        
	}
	
    public void Begin()
    {
        StartCoroutine(deactivate());
    }

    IEnumerator deactivate()
    {
        yield return new WaitForSeconds(time);
        transform.gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
