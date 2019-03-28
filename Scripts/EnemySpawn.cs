using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public bool active;
    public ElementType.Type enemyType;
    public Transform spawn;
    public GameObject currentEnemy;
    public SpawnManager spawnManager;

    public bool flySpot;

	// Use this for initialization
	void Start () {
        currentEnemy = null;
        StartCoroutine(Check());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Check()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(3);
            if (currentEnemy != null)
            {
                if (!currentEnemy.activeSelf)
                {
                    currentEnemy = null;
                    spawnManager.removeMe(this);
                }
            }
        }
    }


}
