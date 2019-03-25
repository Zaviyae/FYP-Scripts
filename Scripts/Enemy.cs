﻿using Exploder.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    public List<GameObject> friendlies;

    private int baseHealth, baseDamage;
    public int currentHealth, currentDamage;

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
    public GameObject[] elementalBlastObjects;

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

    public Transform LAUNCHPOINT;

    //for attacking
    public Transform spawnPos;
    public GameObject shootPrefab;
    private Player player;


    public GameObject shield, overpower;
    public bool overpowered, shielded, spawned;

    public string BrainState = "NEUTRAL";

    public GameObject[] ArcherWeapons, WarriorWeapons, WarriorShields, MageWeapons, MageShields;
    public GameObject[] ArcherHeads, WarriorHeads, MageHeads;
    public GameObject[] ArcherBacks, WarriorBacks, MageBacks;

    
    
    private NavMeshObstacle obstacle;

    public EnemyProfiles myProfile;


    public bool NONMOVINGENEMY = true;

    void Start()
    {
        BrainState = "NEUTRAL";

        friendlies = new List<GameObject>();
        
        obstacle = GetComponent<NavMeshObstacle>();
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        outlineScript = GetComponent<Outline>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (NONMOVINGENEMY)
        {
            obstacle.enabled = false;
            navAgent.enabled = false;
        }


        StartCoroutine(BrainTick());
        StartCoroutine(InformCheck());
       
    }

    void Shield()
    {
        if (!shielded)
        {
            shielded = true;
            shield.SetActive(true);
            anim.SetBool("Spinning", true);
            StartCoroutine(ShieldCountDown(10f));
        }
    }

    void RushPlayer()
    {
        navAgent.enabled = true;
        navAgent.stoppingDistance = 0f;

        navAgent.speed = 7f;
        anim.SetBool("Rolling", true);
    }

    void OverPower()
    {
        overpowered = true;
        overpower.SetActive(true);
        StartCoroutine(OverPowerCount(20f));

    }

    public void SpellTrigger()
    {
        GameObject fireball = spawnManager.fireballPool.get();
        fireball.transform.position = LAUNCHPOINT.position;
        fireball.transform.rotation = LAUNCHPOINT.rotation;
        fireball.GetComponentInChildren<RFX1_TransformMotion>().Target = player.gameObject;
        fireball.transform.LookAt(player.transform.position);
        fireball.SetActive(true);
        fireball.GetComponent<DisableAfter>().Begin();

    }

    void HandleAI()
    {

        if (reachedDistance)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
           // transform.LookAt(player.transform);

        }
    }

    IEnumerator ShieldCountDown(float s)
    {
        yield return new WaitForSeconds(s);
        shielded = false;
        shield.SetActive(false);
        BrainState = "NEUTRAL";
        anim.SetBool("Spinning", false);

    }

    IEnumerator OverPowerCount(float s)
    {
        yield return new WaitForSeconds(s);
        overpowered = false;
        overpower.SetActive(false);
        BrainState = "NEUTRAL";

    }

    IEnumerator InformCheck()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(0.5f);
            friendlies = spawnManager.inform();
                
                
            
        }
    }

    public GameObject NearestEnemy()
    {
        float dist = Mathf.Infinity;
        GameObject tempE = null;
        foreach (GameObject e in friendlies.ToArray())
        {
            float tempdis = Vector3.Distance(transform.position, e.transform.position);
            if(tempdis < dist && tempdis != 0)
            {
                dist = tempdis;
                tempE = e;
            }
            
        }
        return tempE;
    }
    IEnumerator BrainTick()
    {
        for (; ; )
        {
            if (!NONMOVINGENEMY)
            {
                if (pathComplete() || navAgent.enabled == false) { BrainState = "NEUTRAL"; } else { BrainState = "MOVING"; }
                bool changedstate = false;

                //thoughts and decisions

                int playerScore = player.currentScore;
                bool playerShield = player.shieldActive;



                if (BrainState == "NEUTRAL")
                {

                    if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", true); }
                    //SHIELD?

                    int chance = Random.Range(0, 10);
                    if (playerScore >= 8)
                    {
                        if (chance == playerScore)
                        {
                            BrainState = "SHIELDING";
                            Shield();
                            changedstate = true;
                        }
                    }

                    //RUN AT PLAYER?
                    chance = Random.Range(0, 100);
                    if (playerShield)
                        chance = Random.Range(0, 5);

                    if (chance == 3)
                    {
                        BrainState = "RUSHING";
                        RushPlayer();
                        changedstate = true;
                    }

                    //OVERPOWER?
                    chance = Random.Range(0, 200);
                    if (chance == 2)
                    {
                        changedstate = true;
                        OverPower();
                    }

                    //DEMON
                    chance = Random.Range(0, 60);
                    if (chance == 10)
                    {
                        changedstate = true;
                        Morph();

                    }




                    if (!changedstate)
                    {
                        chance = Random.Range(0, 2);
                        if (chance == 1)
                        {
                            //attack
                            anim.SetTrigger(myProfile.attack1);
                            if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }

                        }
                    }
                    else
                    {
                        if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }
                    }
                }
                else
                {
                    if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }


                }




                //score


                if (overpowered)
                {
                    yield return new WaitForSeconds(1);
                }

                yield return new WaitForSeconds(Random.Range(3, 7));
            }
            else
            {
                transform.LookAt(player.transform.position);
                bool changedstate = false;

                //thoughts and decisions

                int playerScore = player.currentScore;
                bool playerShield = player.shieldActive;

                if (BrainState == "NEUTRAL")
                {

                    if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", true); }
                    //SHIELD?

                    int chance = Random.Range(0, 10);
                    if (playerScore >= 8)
                    {
                        if (chance == playerScore)
                        {
                            BrainState = "SHIELDING";
                            Shield();
                            changedstate = true;
                        }
                    }


                    //OVERPOWER?
                    chance = Random.Range(0, 200);
                    if (chance == 2)
                    {
                        changedstate = true;
                        OverPower();
                    }

                    //DEMON
                    chance = Random.Range(0, 80);
                    if (chance == 10)
                    {
                        changedstate = true;
                        Morph();

                    }

                    if (!changedstate)
                    {
                        chance = Random.Range(0, 2);
                        if (chance == 1)
                        {
                            //attack
                            anim.SetTrigger(myProfile.attack1);
                            if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }

                        }
                    }
                    else
                    {
                        if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }
                    }
                }


                yield return new WaitForSeconds(Random.Range(4, 14));
            }
        }
    }


    public void SpawnArrow()
    {
        GameObject newProj = Instantiate(shootPrefab, spawnPos.position, spawnPos.rotation);
        //newProj.GetComponent<RFX1_Target>().Target = player.transform.gameObject;

        newProj.GetComponent<Projectile>().Override(ElementType.Type.Force);
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

    public void spawnObject(int id, bool player)
    {
        
        blastObjects[id].SetActive(false);
        blastObjects[id].SetActive(true);
        blastObjects[id].GetComponent<TargetBlast>().target = this.gameObject;
        blastObjects[id].GetComponent<TargetBlast>().SetUp();
        blastObjects[id].GetComponent<TargetBlast>().playerControlled = player;
    }

    public void spawnObject(Skill skill, bool player)
    {
        if (skill.useSkillID)
        {
            elementalBlastObjects[skill.skillID].SetActive(false);
            elementalBlastObjects[skill.skillID].SetActive(true);
            elementalBlastObjects[skill.skillID].GetComponent<TargetBlast>().target = this.gameObject;
            elementalBlastObjects[skill.skillID].GetComponent<TargetBlast>().SetUp();
            elementalBlastObjects[skill.skillID].GetComponent<TargetBlast>().playerControlled = player;
        }
        else
        {
            if(skill.skillName == "Waterbound")
            {
                Airbound(5);
            }
        }
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
                transform.LookAt(player.transform.position);
                return true;
            }
        }

        return false;
    }

    bool playerCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= myProfile.baseDistance -1)
        {
            return true;
        }
        
        return false;
    }

    void Update()
    {
        if (!NONMOVINGENEMY)
        {
            if (navAgent.enabled)
            {
                reachedDistance = pathComplete();
            }

            if (reachedDistance)
            {
                navAgent.enabled = false;
                obstacle.enabled = true;

            }

            if (!playerCheck())
            {
                navAgent.enabled = true;
                obstacle.enabled = false;
            }


            HandleAI();


            if (targetted)
            {
                outlineScript.enabled = true;
            }
            else
            {
                outlineScript.enabled = false;
            }

            if (!rooted && !frozen && !bound && spawned)
            {

                if (!navAgent)
                    return;
                if (!navAgent.enabled)
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

        if (NONMOVINGENEMY)
        {

        }
    }


    public void TakeDamage(int damage, ElementType.Type el)
    {
        float dF = damage;
        damage = Mathf.RoundToInt(dF *= ElementType.getDamageModifier(el, elementType));

        if (currentHealth <= 0) return;
        
        hpScript.ChangeHP(-damage, hpSpawnLoc.position, Vector3.up, 20f, damage.ToString());
        currentHealth -= damage;
        anim.SetTrigger("TakeDamage");

        if (currentHealth <= 0)
        {
            if (!NONMOVINGENEMY)
            {
                lockPos = transform;
                navAgent.speed = 0f;
                navAgent.isStopped = true;
            }
            currentHealth = 0;
            anim.SetTrigger("Die");
            StartCoroutine(Die());
        }


    }

    public void Explode()
    {
        spawnManager.Deceased(this.gameObject);
    }

    void disableAll()
    {
        foreach (GameObject o in ArcherWeapons)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in ArcherHeads)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in ArcherBacks)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in WarriorWeapons)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in WarriorBacks)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in WarriorHeads)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in WarriorShields)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in MageWeapons)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in MageBacks)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in MageHeads)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in MageShields)
        {
            o.SetActive(false);
        }
    }
    public void SetUp(EnemyProfiles profile)
    {
        myProfile = profile;
        int level = profile.Level;


        disableAll();
        switch (profile.myClass)
        {
            case EnemyProfiles.Class.Archer:


                ArcherWeapons[level - 1].SetActive(true);

             
                    ArcherHeads[level - 1].SetActive(true);

          
                    ArcherBacks[level - 1].SetActive(true);

                break;

            case EnemyProfiles.Class.Swordie:



                WarriorBacks[level - 1].SetActive(true);

                
                    WarriorHeads[level - 1].SetActive(true);

                
                    WarriorShields[level - 1].SetActive(true);

               
                    WarriorWeapons[level - 1].SetActive(true);

                break;
            case EnemyProfiles.Class.Mage:




                MageBacks[level - 1].SetActive(true);


                MageHeads[level - 1].SetActive(true);


                MageShields[level - 1].SetActive(true);


                MageWeapons[level - 1].SetActive(true);

                break;
        }
       
        transform.localScale = new Vector3(profile.baseSize, profile.baseSize, profile.baseSize);

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


        baseHealth = profile.maxHealth;
        currentHealth = baseHealth;
        hpScript.HP = currentHealth;



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

        if (!NONMOVINGENEMY)
        {
            maxSpeed = profile.baseSpeed;
            navAgent.speed = maxSpeed;
            navAgent.stoppingDistance = profile.randomDistance;
            navAgent.isStopped = true;
        }

        

        anim.SetTrigger("Spawn");
      
    }


    public void SpawnAnimFinished()
    {
        spawned = true;
        if (!NONMOVINGENEMY)
        {
            navAgent.isStopped = false;
        }
    }

    void setMesh()
    {


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
        

        yield return new WaitForSeconds(2f);
       
    }

    public void AnimDeath()
    {
        deathEffect.SetActive(false);
        spawnManager.Deceased(this.gameObject);
 
    }
}
