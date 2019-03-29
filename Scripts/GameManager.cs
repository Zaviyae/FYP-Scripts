using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject portalPrefab;
    public float portalSpawnTime;

    public Transform point;
	void Start () {
        
        StartCoroutine(RandomSpawn());
	}
	
	void Update () {
		
	}

    IEnumerator RandomSpawn()
    {
        for(; ; )
        {
            if(portalSpawnTime > 0f)
            {
                yield return new WaitForSeconds(portalSpawnTime);

            }
            else
            {
                yield return new WaitForSeconds(Random.Range(20, 80));
            }

            //int randomx = Random.Range(-10, 10);
            //Vector3 spawnLoc = new Vector3(randomx, 5.84f, 4.32f);



            GameObject port = Instantiate(portalPrefab, point.position, portalPrefab.transform.rotation);
            SpawnItem portalSpawn = port.GetComponentInChildren<SpawnItem>();

            portalSpawn.setUp();
        }
    }
}
