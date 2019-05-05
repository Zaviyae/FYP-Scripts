// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="Enemy.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Class Enemy. Attached to all enemies in the scene.
/// </summary>
public class Enemy : MonoBehaviour
{

    /// <summary>
    /// Enemy objects nearby this one.
    /// </summary>
    public List<GameObject> friendlies;

    /// <summary>
    /// The base health, damage
    /// </summary>
    private int baseHealth, baseDamage;
    /// <summary>
    /// The current health, damage
    /// </summary>
    public int currentHealth, currentDamage;

    /// <summary>
    /// The element type of this enemy
    /// </summary>
    public ElementType.Type elementType;
    /// <summary>
    /// The hp script. Used for displaying hits
    /// </summary>
    public HPScript hpScript;

    /// <summary>
    /// Materials available for this enemy.
    /// </summary>
    public Material blue, red, purple;
    /// <summary>
    /// The nav agent attached to this enemy (used for movement.)
    /// </summary>
    public NavMeshAgent navAgent;
    /// <summary>
    /// Where hit effects will spawn
    /// </summary>
    public Transform hpSpawnLoc;
    /// <summary>
    /// The animator attached to this enemy.
    /// </summary>
    public Animator anim;
    /// <summary>
    ///If the enemy has reached their destination
    /// </summary>
    bool atDesination;
    /// <summary>
    /// The base mesh renderer
    /// </summary>
    public SkinnedMeshRenderer baseMeshRenderer;
    /// <summary>
    /// The outline script
    /// </summary>
    private Outline outlineScript;
    /// <summary>
    /// The shoot position (where to shoot from)
    /// </summary>
    public GameObject shootPos;

    /// <summary>
    /// The stun circles 
    /// </summary>
    public GameObject stunCircles;

    /// <summary>
    /// The death effect
    /// </summary>
    public GameObject deathEffect;

    /// <summary>
    /// The dementor object (effect)
    /// </summary>
    public GameObject dementor;

    /// <summary>
    /// The comet point
    /// </summary>
    public Transform cometPoint;

    /// <summary>
    /// The root seconds, freeze seconds
    /// </summary>
    public float rootSeconds, freezeSeconds;
    /// <summary>
    /// The target blast points
    /// </summary>
    public Transform[] blastPoints;

    /// <summary>
    /// The target blast objects
    /// </summary>
    public GameObject[] blastObjects;
    /// <summary>
    /// The elemental target blast (Skills) objects
    /// </summary>
    public GameObject[] elementalBlastObjects;

    /// <summary>
    /// The spawn manager
    /// </summary>
    public SpawnManager spawnManager;
    /// <summary>
    /// Is this enemy rooted or frozen?
    /// </summary>
    public bool rooted = false, frozen = false;
    /// <summary>
    /// Is this enemy targetted by the player.
    /// </summary>
    public bool targetted;

    /// <summary>
    /// The position to lock this enemy to.
    /// </summary>
    public Transform lockPos;

    /// <summary>
    /// The maximum speed of this enemy.
    /// </summary>
    public float maxSpeed;

    /// <summary>
    /// Is this enemy position locked.
    /// </summary>
    public bool lockedOn;
    /// <summary>
    /// Triggers for morphing and bound effects for the enemy.
    /// </summary>
    public bool morph, bound;
    /// <summary>
    /// The bound seconds
    /// </summary>
    public float boundSeconds;
    /// <summary>
    /// A skill effect for water ball.
    /// </summary>
    public GameObject waterBall;

    /// <summary>
    /// Has the enemy reached the distance from the player
    /// </summary>
    public bool reachedDistance;
    /// <summary>
    /// The old position, old rotation
    /// </summary>
    private Vector3 oldPos, oldRot;

    /// <summary>
    /// The launchpoint
    /// </summary>
    public Transform LAUNCHPOINT;

    /// <summary>
    /// The spawn position
    /// </summary>
    public Transform spawnPos;
    /// <summary>
    /// The shoot prefab
    /// </summary>
    public GameObject shootPrefab;
    /// <summary>
    /// The player
    /// </summary>
    private Player player;


    /// <summary>
    /// The shields available for the enemy.
    /// </summary>
    public GameObject shield, overpower,elementshield, elementshieldB, elementshieldP, elementshieldR;
  
    public bool overpowered, shielded, spawned;

    /// <summary>
    /// The brain state
    /// </summary>
    public string BrainState = "NEUTRAL";

    /// <summary>
    /// The archer weapons
    /// </summary>
    public GameObject[] ArcherWeapons, WarriorWeapons, WarriorShields, MageWeapons, MageShields, HealerWeapons;
    /// <summary>
    /// The archer heads
    /// </summary>
    public GameObject[] ArcherHeads, WarriorHeads, MageHeads, HealerHeads;
    /// <summary>
    /// The archer backs
    /// </summary>
    public GameObject[] ArcherBacks, WarriorBacks, MageBacks, HealerBacks;


    /// <summary>
    /// The heal effect
    /// </summary>
    public GameObject healEffect;

    /// <summary>
    /// If this enemy is a navmesh obstacle.
    /// </summary>
    private NavMeshObstacle obstacle;

    /// <summary>
    /// This enemy's enemy profile
    /// </summary>
    public EnemyProfiles myProfile;

    /// <summary>
    /// The time alive
    /// </summary>
    public int timeAlive;
    /// <summary>
    /// Is this a support enemy
    /// </summary>
    public bool supportEnemy;
    /// <summary>
    /// Is this a nonmovingenemy
    /// </summary>
    public bool NONMOVINGENEMY = true;
    /// <summary>
    /// Is this enemy dead
    /// </summary>
    bool dead = false;

    /// <summary>
    /// Starts this instance. Sets up basic references.
    /// </summary>
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

       

    }

    /// <summary>
    /// Shields this enemy.
    /// </summary>
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

    /// <summary>
    /// Rushes the player.
    /// </summary>
    void RushPlayer()
    {
        navAgent.enabled = true;
        navAgent.stoppingDistance = 0f;

        navAgent.speed = 7f;
        anim.SetBool("Rolling", true);
    }

    /// <summary>
    /// Cast faster , a power up.
    /// </summary>
    void OverPower()
    {
        overpowered = true;
        overpower.SetActive(true);
        StartCoroutine(OverPowerCount(20f));

    }

    /// <summary>
    /// Cast fireball.
    /// </summary>
    public void SpellTrigger()
    {
        GameObject fireball = spawnManager.fireballPool.get();
        fireball.transform.position = LAUNCHPOINT.position;
        fireball.transform.rotation = LAUNCHPOINT.rotation;
        GameObject targ = GameObject.Instantiate(new GameObject());
        targ.transform.position = player.transform.position;
        fireball.GetComponentInChildren<RFX1_TransformMotion>().Target = targ;
        fireball.GetComponentInChildren<RFX1_TransformMotion>().player = player;
        Destroy(targ);
        fireball.transform.LookAt(player.transform.position);
        fireball.SetActive(true);
        fireball.GetComponent<DisableAfter>().Begin();

    }

    /// <summary>
    /// Handles the ai movement animation.
    /// </summary>
    void HandleAI()
    {

        if (reachedDistance)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
           // transform.LookAt(player.transform);

        }
    }

    /// <summary>
    /// Count down the shield seconds.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator ShieldCountDown(float s)
    {
        yield return new WaitForSeconds(s);
        shielded = false;
        shield.SetActive(false);
        BrainState = "NEUTRAL";
        anim.SetBool("Spinning", false);

    }

    /// <summary>
    /// Count down the over power buff seconds
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator OverPowerCount(float s)
    {
        yield return new WaitForSeconds(s);
        overpowered = false;
        overpower.SetActive(false);
        BrainState = "NEUTRAL";

    }

    /// <summary>
    /// Gets the nearby enemies from the spawn manager.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator InformCheck()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(0.5f);
            friendlies = spawnManager.inform();

            transform.LookAt(player.transform);
        }
    }

    /// <summary>
    /// Gets nearest enemy
    /// </summary>
    /// <returns>GameObject.</returns>
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

    /// <summary>
    /// Gets random enemy
    /// </summary>
    /// <returns>GameObject.</returns>
    public GameObject RandomEnemy()
    {
        return friendlies[Random.Range(0, friendlies.Count - 1)];
    }

    /// <summary>
    /// Increments time alive every second
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator AliveCount()
    {
        for(; ; )
        {
            yield return new WaitForSecondsRealtime(1);
            timeAlive++;
        }
    }
    /// <summary>
    /// Brain tick, handles states
    /// </summary>
    /// <returns>IEnumerator.</returns>
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
                    anim.SetBool("Healing", false);
                    if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", true); }

                    /*
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
                    */
                    /*
                    //OVERPOWER?
                    chance = Random.Range(0, 200);
                    if (chance == 2)
                    {
                        changedstate = true;
                        OverPower();
                    }
                    */
                    //DEMON
                    int chance;
                    chance = Random.Range(0, 80);
                    if (chance == 10 && !supportEnemy)
                    {
                        changedstate = true;
                        Morph();

                    }

                    if (!changedstate && BrainState == "NEUTRAL")
                    {
                        chance = Random.Range(0, 3);
                        if (supportEnemy) chance = 1;
                        if (chance == 1)
                        {
                            //attack
                            if (!supportEnemy)
                            {
                                anim.SetTrigger(myProfile.attack1);
                                if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }
                            }
                            else
                            {
                                //heal others
                                BrainState = "HEALING";
                                
                            }

                        }
                    }
                    else
                    {
                        if (myProfile.myClass == EnemyProfiles.Class.Archer) { anim.SetBool("Aiming", false); }
                    }
                }

                if (overpower) { yield return new WaitForSeconds(1); }
                yield return new WaitForSeconds(Random.Range(4, 10));
            }
        }
    }


    /// <summary>
    /// Healing tick, for support enemy. heals lowest hp enemy.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator HealTick()
    {
        for(; ; )
        {
            if (BrainState == "HEALING")
            {
                float lowesthp = Mathf.Infinity;
                Enemy enemyToHeal = null;
                foreach (GameObject e in friendlies)
                {
                    if (e != enemyToHeal) e.GetComponent<Enemy>().healEffect.SetActive(false);
                    float tmpHealth = e.GetComponent<Enemy>().currentHealth;
                    if(tmpHealth < lowesthp)
                    {
                        lowesthp = tmpHealth;
                        enemyToHeal = e.GetComponent<Enemy>();
                    }
                }
                if (enemyToHeal != null) { if (enemyToHeal.myProfile.maxHealth == enemyToHeal.currentHealth) { } else { enemyToHeal.Heal(2, Color.green); enemyToHeal.healEffect.SetActive(true); anim.SetBool("Healing", true); } }
            }

            if (Random.Range(0, 300) == 1) BrainState = "NEUTRAL";

            yield return new WaitForSeconds(.4f);
        }
    }

    /// <summary>
    /// Heals the specified health.
    /// </summary>
    /// <param name="health">The health.</param>
    /// <param name="color">The color.</param>
    public void Heal(int health, Color color)
    {
        if (currentHealth <= 0) return;
        if (currentHealth == myProfile.maxHealth) return;
        hpScript.ChangeHP(+health, hpSpawnLoc.position, Vector3.up, 2f, color, "+ " + health.ToString());

        currentHealth += health;

        if (currentHealth > myProfile.maxHealth) currentHealth = myProfile.maxHealth;
      
    }
    /// <summary>
    /// Spawns the arrow.
    /// </summary>
    public void SpawnArrow()
    {
        GameObject newProj = Instantiate(shootPrefab, spawnPos.position, spawnPos.rotation);
        //newProj.GetComponent<RFX1_Target>().Target = player.transform.gameObject;

        newProj.GetComponent<Projectile>().Override(ElementType.Type.Purple);
    }

    /// <summary>
    /// If the position is locked on.
    /// </summary>
    public void LockOn()
    {
        lockedOn = true;
    }

    /// <summary>
    /// Roots the enemy for s seconds
    /// </summary>
    /// <param name="s">The s.</param>
    public void Root(float s)
    {
        rootSeconds = s;
        rooted = true;


        stunCircles.SetActive(true);

        anim.SetBool("Rooted", true);
    }

    /// <summary>
    /// Freezes the enemy for s seconds.
    /// </summary>
    /// <param name="s">The s.</param>
    public void Freeze(float s)
    {
        if (!NONMOVINGENEMY)
        {
            lockPos = transform;
            navAgent.speed = 0f;
            navAgent.isStopped = true;
        }
        anim.speed = 0;
        freezeSeconds = s;
        frozen = true;



    }

    /// <summary>
    /// Airbounds the enemy for s seconds.
    /// </summary>
    /// <param name="s">The s.</param>
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

    /// <summary>
    ///Countdown bound seconds
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>IEnumerator.</returns>
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

    /// <summary>
    /// Spawns the object.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="player">if set to <c>true</c> [player].</param>
    public void spawnObject(int id, bool player)
    {
        
        blastObjects[id].SetActive(false);
        blastObjects[id].SetActive(true);
        blastObjects[id].GetComponent<TargetBlast>().target = this.gameObject;
        blastObjects[id].GetComponent<TargetBlast>().SetUp();
        blastObjects[id].GetComponent<TargetBlast>().playerControlled = player;
    }

    /// <summary>
    /// Spawns the object.
    /// </summary>
    /// <param name="skill">The skill.</param>
    /// <param name="player">if set to <c>true</c> [player].</param>
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

    /// <summary>
    /// Morphes this instance.
    /// </summary>
    public void Morph()
    {
        print("Morph!");
        dementor.SetActive(true);
        dementor.GetComponent<RFX1_Target>().Target = GameObject.FindGameObjectWithTag("Player");
        dementor.transform.parent = null;
        ReportDeath();
    }

    /// <summary>
    /// If the enemy has reached the player
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

    /// <summary>
    /// Checks distance to player
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    bool playerCheck()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= myProfile.baseDistance -1)
        {
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Called once per frame. Handled rooting, target outlines and animations.
    /// </summary>
    void Update()
    {
   
        if (targetted)
        {
            outlineScript.enabled = true;
        }
        else
        {
            outlineScript.enabled = false;
        }


        if (rooted)
        {
            rootSeconds -= 1 * Time.fixedDeltaTime;
            if (rootSeconds <= 0)
            {
                rooted = false;
                anim.SetBool("Rooted", false);
                stunCircles.SetActive(false);
            }

            if (!NONMOVINGENEMY)
            {
                anim.SetBool("Running", false);
                anim.SetBool("Walking", false);
                navAgent.destination = transform.position;
            }
        }

        if (frozen)
        {
            if (!NONMOVINGENEMY)
            {
                transform.position = lockPos.position;

                navAgent.destination = transform.position;
            }


            freezeSeconds -= 1 * Time.fixedDeltaTime;
            if (freezeSeconds <= 0)
            {
                frozen = false;
                anim.speed = 1f;

            }
        }


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


            }
        }

        if (NONMOVINGENEMY)
        {

        }
    }

    /// <summary>
    /// Takes the damage passed.
    /// </summary>
    /// <param name="damage">The damage.</param>
    /// <param name="color">The color.</param>
    public void TakeDamage(int damage, Color color)
    {


        if (currentHealth <= 0) return;
        currentHealth -= damage;
        hpScript.ChangeHP(-damage, hpSpawnLoc.position, Vector3.up, 2f, color, damage.ToString());
        
        anim.SetTrigger("TakeDamage");

        CheckDeath();
    }

    /// <summary>
    /// Takes the damage passed of element type.
    /// </summary>
    /// <param name="damage">The damage.</param>
    /// <param name="el">The el.</param>
    public void TakeDamage(int damage, ElementType.Type el)
    {
        float dF = damage;
        damage = Mathf.RoundToInt(dF *= ElementType.getDamageModifier(el, elementType));


        CheckDeath();
        if (currentHealth <= 0) return;
        hpScript.ChangeHP(-damage, hpSpawnLoc.position, Vector3.up, 20f, damage.ToString());
        currentHealth -= damage;
        anim.SetTrigger("TakeDamage");

        CheckDeath();
    }

    /// <summary>
    /// Checks if this enemy has died.
    /// </summary>
    void CheckDeath()
    {
        
        if (currentHealth <= 0)
        {
            
            if (!NONMOVINGENEMY)
            {
                lockPos = transform;
                navAgent.speed = 0f;
                navAgent.isStopped = true;
            }
            if (!dead)
            {
                player.AddScore(100 - timeAlive);
                currentHealth = 0;
                anim.SetTrigger("Die");
                StartCoroutine(Die());
            }
            dead = true;
        }
    }

    /// <summary>
    /// Reports the death to spawn manager.
    /// </summary>
    public void ReportDeath()
    {
        spawnManager.EnemyDeceased(this.gameObject);
    }

    /// <summary>
    /// Disables all customisation objects on the enemy.
    /// </summary>
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
        foreach (GameObject o in HealerBacks)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in HealerHeads)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in HealerWeapons)
        {
            o.SetActive(false);
        }
    }

    /// <summary>
    /// Sets up the enemy with all customisation and element settings.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public void SetUp(EnemyProfiles profile)
    {
       
        elementshieldB.SetActive(false);
        elementshieldP.SetActive(false);
        elementshieldR.SetActive(false);
        elementshield = null;

      player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myProfile = profile;
        int level = profile.Level;


        disableAll();
        switch (profile.myClass)
        {
            case EnemyProfiles.Class.Healer:
                HealerWeapons[level - 1].SetActive(true);
                HealerHeads[level - 1].SetActive(true);
                HealerBacks[level - 1].SetActive(true);
                supportEnemy = true;
                break;

            case EnemyProfiles.Class.Archer:


                ArcherWeapons[level - 1].SetActive(true);

             
                    ArcherHeads[level - 1].SetActive(true);

          
                    ArcherBacks[level - 1].SetActive(true);
                supportEnemy = false;

                break;

            case EnemyProfiles.Class.Mage:




                MageBacks[level - 1].SetActive(true);


                MageHeads[level - 1].SetActive(true);


                MageShields[level - 1].SetActive(true);


                MageWeapons[level - 1].SetActive(true);
                supportEnemy = false;

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



        int elementInt = Random.Range(0, 3);

        switch (elementInt)
        {
            case 0:
                elementType = ElementType.Type.Purple;
                elementshield = elementshieldP;
                break;
            case 1:
                elementType = ElementType.Type.Red;
                elementshield = elementshieldR;
                break;
            case 2:
                elementType = ElementType.Type.Blue;
                elementshield = elementshieldB;
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
        dead = false;

        anim.SetTrigger("Spawn");
        StartCoroutine(HealTick());
        timeAlive = 0;
        StartCoroutine(AliveCount());
        waterBall.SetActive(false);
        BrainState = "NEUTRAL";
    }


    /// <summary>
    /// If the spawn animaton has finished, this method is triggered.
    /// </summary>
    public void SpawnAnimFinished()
    {
        spawned = true;
        if (!NONMOVINGENEMY)
        {
            navAgent.isStopped = false;
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(BrainTick());
            StartCoroutine(InformCheck());
        }
        BrainState = "NEUTRAL";
    }

    /// <summary>
    /// Sets the mesh to the element type
    /// </summary>
    void setMesh()
    {


        bool neutral = false;

        switch (elementType)
        {
            case ElementType.Type.Purple:
                baseMeshRenderer.material = purple;
                break;
            case ElementType.Type.Red:
                baseMeshRenderer.material = red;
                break;
            case ElementType.Type.Blue:
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

    /// <summary>
    /// Die after 2 seconds
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Die()
    {
        dead = true;
        deathEffect.SetActive(true);
     

        yield return new WaitForSeconds(2f);
       
    }

    /// <summary>
    /// Triggered by the death animation.
    /// </summary>
    public void AnimDeath()
    {
        deathEffect.SetActive(false);
        spawnManager.EnemyDeceased(this.gameObject);
    
    }

    /// <summary>
    /// Activates or de activates element shield.
    /// </summary>
    /// <param name="t">if set to <c>true</c> [t].</param>
    public void ElementShield(bool t)
    {
        elementshield.SetActive(t);
    }

}
