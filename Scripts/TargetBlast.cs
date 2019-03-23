using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlast : MonoBehaviour {

    public bool playerControlled;

    public GameObject target;

    public int damage;
    public ElementType.Type type;
    public float yIncrease;
    public float seconds, timetilldamage;
    public bool instantDamage, collisionDamage;
    bool hit;
    Enemy enemy;
    Player player;

    public enum blastType
    {
        ROOT, FREEZE, DAMAGE, SUSPEND
    }


    public blastType myType;
    public bool useCustomLocation, parentcustomlocation, useCustomObject;
    public int customLocationID, customObjectID;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

        var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if (raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
        SetUp();
        }

    public void SetUp()
    {
        hit = false;
        enemy = target.GetComponent<Enemy>();
        if (!useCustomObject)
        {
            if (!useCustomLocation)
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + yIncrease, target.transform.position.z);
            }
            else
            {
                transform.position = enemy.blastPoints[customLocationID].transform.position;
                transform.rotation = enemy.blastPoints[customLocationID].transform.rotation;
                if (parentcustomlocation)
                {
                    transform.parent = enemy.blastPoints[customLocationID];
                }
            }
        }
        else
        {

            CustomObjectSpawn(customObjectID);

        }

        if (myType == blastType.ROOT)
        {
            // transform.parent = target.transform;
            enemy.Root(seconds);
            if (instantDamage)
            {
                enemy.TakeDamage(player.calcDamage(2), type);
            }
        }

        if (myType == blastType.DAMAGE)
        {
            //transform.parent = target.transform;
            enemy.Root(seconds);
            if (instantDamage)
            {
                StartCoroutine(Damage(timetilldamage));
            }

        }

        if (myType == blastType.SUSPEND)
        {
            enemy.Airbound(5);

        }

        if (myType == blastType.FREEZE)
        {
            enemy.Freeze(seconds);
            if (instantDamage)
            {
                enemy.TakeDamage(player.calcDamage(2), type);
            }
            else
            {
                if (!collisionDamage)
                {
                    StartCoroutine(Damage(timetilldamage));
                }
                
            }
        }

        
    }
   // private void OnEnable()
   // {
       
   // }

    public void CustomObjectSpawn(int id)
    {
        enemy.blastObjects[id].SetActive(false);
        enemy.blastObjects[id].SetActive(true);

    }

	void Update () {
		
	}



    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        if (!hit && collisionDamage)
        {
            hit = true;
            if (e.HitGameObject.tag == "Enemy")
            {
                Enemy enemy = e.HitGameObject.transform.GetComponent<Enemy>();

                if (enemy)
                {
                    StartCoroutine(Damage(timetilldamage));

                }
            }
            else
            {
                if (e.HitGameObject.transform.root.tag == "Enemy")
                {
                    Enemy enemy = e.HitGameObject.transform.root.GetComponent<Enemy>();

                    if (enemy)
                    {
                        StartCoroutine(Damage(timetilldamage));
                    }
                }
            }

            Debug.Log("hit - " + name);
        }
        
    }

    public void Hit()
    {
        
        print("Target hit");
        StartCoroutine(Damage(timetilldamage));
       

    }



    IEnumerator Damage(float time)
    {
        yield return new WaitForSeconds(time);
        enemy.anim.speed = 1f;

        if (playerControlled)
        {
            enemy.TakeDamage(player.calcDamage(2), type);
        }
        else
        {
            enemy.TakeDamage(damage, type);
        }
        
    }
}
