using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour {

    RFX4_ScaleCurves scaleCurves;
    //public GameObject obj;
    float time;
    bool objFloat;
    public GameObject spawnPrefab;

	// Use this for initialization
	void Start () {

        transform.root.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        scaleCurves = GetComponent<RFX4_ScaleCurves>();
        time = scaleCurves.GraphTimeMultiplier / 2;

	}
	
    public void setUp()
    {
        transform.root.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        scaleCurves = GetComponent<RFX4_ScaleCurves>();
        time = scaleCurves.GraphTimeMultiplier / 2;
        objFloat = false;
    }
	// Update is called once per frame
	void Update () {

        if (time <= 0 && !objFloat)
        {
            Release();
        }
        else
        {
            time -= 1 * Time.fixedDeltaTime;
        }



	}

    void Release()
    {
        GameObject obj = Instantiate(spawnPrefab, transform.position, transform.rotation, transform);
        objFloat = true;
        print("released!");
        obj.SetActive(true);
        
        obj.GetComponent<PowerUpThrow>().floating();
    }
}
