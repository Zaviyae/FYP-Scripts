using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : MonoBehaviour {

    public ElementType.Type elementType;
    public GameObject target = null;
    public GameObject[] potentials;
    public List<GameObject> filtered;
    GameObject proj;
    public Transform shootPoint;
    public GameObject blastPrefab;
    public Vector3 movePoint;
    bool started;
    public GameObject player;
    public bool useCustomSkill;
    public float lifeTime;
    public bool close;

    public bool flamethrower;
    public GameObject flame;
   // public int skillID;

    public Skill[] skills;

    // Use this for initialization
    void Start () {
        flamethrower = true;

        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("Check");
        StartCoroutine("Fire");

        if(lifeTime == 0)
        {
            lifeTime = Random.Range(25, 80);
        }
        StartCoroutine(Destroy());
        StartCoroutine(FlameTick());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        flame.SetActive(close);
        if (target)
        {
            started = false;
            transform.LookAt(target.transform);
            if (Vector3.Distance(transform.position, target.transform.position) <= 4)
            {
                Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, pos, 0.2f * Time.deltaTime);
                close = true;
                
            }
            else
            {
                close = false;
                Vector3 pos = new Vector3(target.transform.position.x, target.transform.position.y + 3, target.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, pos, 0.7f * Time.deltaTime);
            }

        }
        else
        {
            close = false;
            if (!started)
            {
                StartCoroutine(RandomMove());
            }
            started = true;

            transform.position = Vector3.Lerp(transform.position, movePoint, 0.5f * Time.deltaTime);
        }
	}

    IEnumerator RandomMove()
    {
        while (!target)
        {
            int sec = Random.Range(5, 15);
 

            int randomx = Random.Range(-42, 42);
            int randomz = Random.Range(-1, 90);

            movePoint = new Vector3(randomx, transform.position.y, randomz);

            yield return new WaitForSeconds(sec);
        }
        
    }

    IEnumerator Check()
    {
        for(; ; )
        {
            if (target != null && !target.activeSelf)
            {
                target = null;
            }

            Search();
            yield return new WaitForSeconds(.2f);
        }
    
    }


    IEnumerator FlameTick()
    {
        for (; ; )
        {
            if (flamethrower)
            {
                yield return new WaitForSeconds(0.15f);
                if (close)
                {
                    target.GetComponent<Enemy>().TakeDamage(3, Color.cyan);
                }
            }
        }
    }

    IEnumerator Fire()
    {
        for (; ; )
        {
            
            if (target)
            {
                if (useCustomSkill)
                {
                    Enemy currentTarget = target.GetComponent<Enemy>();
                    currentTarget.spawnObject(skills[0], false);

                }
                else
                {
                   // GameObject blast = GameObject.Instantiate(blastPrefab);
                   // blast.GetComponent<TargetBlast>().target = target;
                }

                
            }
            float time = Random.Range(7, 20);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifeTime);
        if (target)
            target.GetComponent<Enemy>().lockedOn = false;
        Destroy(gameObject);
    }
    void Search()
    {
  
        potentials = GameObject.FindGameObjectsWithTag("Enemy");

        if (potentials.Length == 0)
        {

        }
        else
        {
            filtered = new List<GameObject>();

            foreach (GameObject g in potentials)
            {
                if (g.GetComponent<Enemy>() && !g.GetComponent<Enemy>().lockedOn)
                {
                    filtered.Add(g);
                }
             
            }

            if (target)
            {
                //nothing
            }
            else
            {
                int randomEnemy = Random.Range(0, filtered.Count);
                target = filtered[randomEnemy];
                target.GetComponent<Enemy>().LockOn();
            }


        }
    }
}
