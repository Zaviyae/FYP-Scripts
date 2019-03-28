using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {

    public int time;
    Player player;
    public SpawnManager spawn;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Summon()
    {
        StartCoroutine(CountDown());
        StartCoroutine(DamageTick());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator DamageTick()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(.4f);
            foreach(GameObject e in spawn.activePool)
            {
                e.GetComponent<Enemy>().TakeDamage(3, Color.yellow);
            }
        }
        
    }

}
