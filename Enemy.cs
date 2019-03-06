using Exploder.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{


    private int baseHealth, baseDamage, baseDefence;
    public int currentHealth, currentDamage, currentDefence = 100;
    public ElementType.Type elementType;
    public HPScript hpScript;

    public Material blue, gold, red;
    public NavMeshAgent navAgent;
    public Transform hpSpawnLoc;
    public Animator anim;
    bool atDesination;
    public SkinnedMeshRenderer baseMeshRenderer;
    private Outline outlineScript;
    public GameObject shootPos;

    public GameObject stunCircles;

    public GameObject deathEffect;

    public GameObject dementor;

    public Transform cometPoint;

    public float rootSeconds, freezeSeconds;
    public Transform[] blastPoints;

    public GameObject[] blastObjects;


    public SpawnManager spawnManager;
    public bool rooted = false, frozen = false;
    public bool targetted;

    public Transform lockPos;

    public float maxSpeed;

    public bool lockedOn;
    public bool morph, bound;
    public float boundSeconds;
    public GameObject waterBall;

    public bool testBound;
    public bool reachedDistance;
    private Vector3 oldPos, oldRot;


    //for attacking
    public Transform spawnPos;
    public GameObject shootPrefab;
    private Player player;

    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        outlineScript = GetComponent<Outline>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(Attack());
    }

    void HandleAI()
    {
        if (reachedDistance)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            transform.LookAt(player.transform);

        }
    }

    IEnumerator Attack()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(5f);

            anim.SetTrigger("Longbow");

            yield return new WaitForSeconds(0.5f);

            if (reachedDistance)
            {
                GameObject newProj = Instantiate(shootPrefab, spawnPos.position, spawnPos.rotation);
                //newProj.GetComponent<RFX1_Target>().Target = player.transform.gameObject;

                newProj.GetComponent<Projectile>().Override(ElementType.Type.Force);
            }
        }
    }

    public void LockOn()
    {
        lockedOn = true;
    }

    public void Root(float s)
    {
        rootSeconds = s;
        rooted = true;


        stunCircles.SetActive(true);

        anim.SetBool("Rooted", true);
    }

    public void Freeze(float s)
    {
        lockPos = transform;
        navAgent.speed = 0f;
        navAgent.isStopped = true;
        anim.speed = 0;
        freezeSeconds = s;
        frozen = true;



    }

    public void Airbound(float s)
    {
        if (!bound)
        {
            navAgent.enabled = false;
            boundSeconds = s;
            bound = true;
            anim.SetBool("Swim", true);
            oldPos = transform.position;
            oldRot = transform.rotation.eulerAngles;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);
            transform.position = newPos;

            Vector3 newRot = new Vector3(-94f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler(newRot);
            print("Attempted airbound");
            waterBall.SetActive(true);
            StartCoroutine(BoundTimer(s));
        }
    }

    IEnumerator BoundTimer(float s)
    {
        yield return new WaitForSeconds(s);
        navAgent.enabled = true;
        boundSeconds = 0;
        bound = false;
        anim.SetBool("Swim", false);
        transform.position = oldPos;
        transform.rotation = Quaternion.Euler(oldRot);
        print("Bound over");
        waterBall.SetActive(false);
    }
    public void spawnObject(int id)
    {
        blastObjects[id].SetActive(false);
        blastObjects[id].SetActive(true);
        blastObjects[id].GetComponent<TargetBlast>().target = this.gameObject;
        blastObjects[id].GetComponent<TargetBlast>().SetUp();
    }

    public void spawnObject(Skill skill)
    {
        blastObjects[skill.skillID].SetActive(false);
        blastObjects[skill.skillID].SetActive(true);
        blastObjects[skill.skillID].GetComponent<TargetBlast>().target = this.gameObject;
        blastObjects[skill.skillID].GetComponent<TargetBlast>().SetUp();
    }

    public void Morph()
    {
        print("Morph!");
        dementor.SetActive(true);
        dementor.GetComponent<RFX1_Target>().Target = GameObject.FindGameObjectWithTag("Player");
        dementor.transform.parent = null;
        Explode();
    }

    protected bool pathComplete()
    {
        if (Vector3.Distance(navAgent.destination, navAgent.transform.position) <= navAgent.stoppingDistance)
        {
            if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {

        reachedDistance = pathComplete();

        HandleAI();


        if (morph)
        {
            Morph();
        }
        if (testBound)
        {
            Airbound(5);
        }
        if (targetted)
        {
            outlineScript.enabled = true;
        }
        else
        {
            outlineScript.enabled = false;
        }

        if (!rooted && !frozen)
        {
            
            if (!navAgent)
                return;
            navAgent.speed = maxSpeed;
            navAgent.isStopped = false;
            navAgent.destination = GameObject.FindGameObjectWithTag("Finish").transform.position;
            float dist = navAgent.remainingDistance;

            if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && navAgent.remainingDistance <= navAgent.stoppingDistance)
            { //Arrived.
                atDesination = true;
                if (navAgent.speed > 0.5f)
                {
                    anim.SetBool("Walking", false);
                }
                if (navAgent.speed > 3f)
                {
                    anim.SetBool("Running", false);
                }
            }
            else
            {
                if (navAgent.speed > 0.5f)
                {
                    anim.SetBool("Walking", true);
                }
                if (navAgent.speed > 3f)
                {
                    anim.SetBool("Running", true);
                }

                atDesination = false;

            }
        }
        else
        {
            if (!navAgent)
                return;

            if (rooted)
            {
                rootSeconds -= 1 * Time.fixedDeltaTime;
                if (rootSeconds <= 0)
                {
                    rooted = false;
                    anim.SetBool("Rooted", false);
                    stunCircles.SetActive(false);
                }


                anim.SetBool("Running", false);
                anim.SetBool("Walking", false);
                navAgent.destination = transform.position;
            }

            if (frozen)
            {
                transform.position = lockPos.position;
                freezeSeconds -= 1 * Time.fixedDeltaTime;
                navAgent.destination = transform.position;

                if (freezeSeconds <= 0)
                {
                    frozen = false;
                    anim.speed = 1f;

                }
            }
        }
    }


    public void TakeDamage(int damage)
    {
        hpScript.ChangeHP(-damage, hpSpawnLoc.position, Vector3.up, 20f, damage.ToString());
        currentHealth -= damage;
        anim.SetTrigger("TakeDamage");
        if (currentHealth <= 0)
        {
            lockPos = transform;
            navAgent.speed = 0f;
            navAgent.isStopped = true;
            currentHealth = 0;
            anim.SetTrigger("Die");
            StartCoroutine(Die());
        }


    }

    public void Explode()
    {
        spawnManager.Deceased(this.gameObject);
    }

    public void SetUp(int health, float speed)
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        lockedOn = false;
        foreach (GameObject o in blastObjects)
        {
            o.SetActive(false);
        }

        rootSeconds = 0f;
        freezeSeconds = 0f;
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        this.gameObject.SetActive(true);
        baseHealth = health;
        currentHealth = baseHealth;
        hpScript.HP = currentHealth;
        navAgent.speed = speed;
        maxSpeed = speed;
        int elementInt = Random.Range(0, 4);
        switch (elementInt)
        {
            case 0:
                elementType = ElementType.Type.Force;
                break;
            case 1:
                elementType = ElementType.Type.Lightning;
                break;
            case 2:
                elementType = ElementType.Type.Neutral;
                break;
            case 3:
                elementType = ElementType.Type.Water;
                break;
            default:
                elementType = ElementType.Type.Neutral;
                break;
        }
        setMesh();
    }


    public void SetUp(int health, float speed, ElementType.Type element)
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        frozen = false;
        lockedOn = false;
        rooted = false;
        anim.speed = 1f;
        stunCircles.SetActive(false);
        rootSeconds = 0f;
        freezeSeconds = 0f;
        foreach (GameObject o in blastObjects)
        {
            o.SetActive(false);
        }

        baseHealth = health;
        currentHealth = baseHealth;
        hpScript.HP = currentHealth;
        elementType = element;
        navAgent.speed = speed;
        maxSpeed = speed;
        setMesh();
    }

    void setMesh()
    {
        /*
        GameObject mesh = null;
        bool neutral = false;
        switch (elementType)
        {
            case ElementType.Type.Force:
                mesh = ForceMesh;
                break;
            case ElementType.Type.Lightning:
                mesh = LightningMesh;
                break;
            case ElementType.Type.Water:
                mesh = WaterMesh;
                break;
            default:
                neutral = true;
                break;
        }
        if (!neutral)
        {
            mesh.SetActive(true);
            mesh.GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect();
        }
        */


        bool neutral = false;

        switch (elementType)
        {
            case ElementType.Type.Force:
                baseMeshRenderer.material = red;
                break;
            case ElementType.Type.Lightning:
                baseMeshRenderer.material = gold;
                break;
            case ElementType.Type.Water:
                baseMeshRenderer.material = blue;
                break;
            default:
                neutral = true;
                break;
        }
        if (!neutral)
        {

        }
    }

    IEnumerator Die()
    {
        deathEffect.SetActive(true);
        //ExploderSingleton.ExploderInstance.ExplodeObject(gameObject);

        yield return new WaitForSeconds(2f);
       
    }

    public void AnimDeath()
    {
        deathEffect.SetActive(false);
        spawnManager.Deceased(this.gameObject);
 
    }
}
