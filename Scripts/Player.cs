// ***********************************************************************
// Assembly         : 
// Author           : zaviy
// Created          : 03-29-2019
//
// Last Modified By : zaviy
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="Player.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class Player.
/// Attached to the player object.
/// </summary>
public class Player : MonoBehaviour {

    /// <summary>
    /// Has the game started
    /// </summary>
    private bool gameStarted;

    /// <summary>
    /// The vr player
    /// </summary>
    private Valve.VR.InteractionSystem.Player vrPlayer;

    /// <summary>
    /// The current score
    /// </summary>
    public int currentScore = 0;
    /// <summary>
    /// The shield
    /// </summary>
    public GameObject shield;

    /// <summary>
    /// The current enemy target
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The red skill set
    /// </summary>
    public GameObject[] LightningSkillSet;
    /// <summary>
    /// The blue skill set
    /// </summary>
    public GameObject[] WaterSkillSet;
    /// <summary>
    /// The purple skill set
    /// </summary>
    public GameObject[] ForceSkillSet;



    /// <summary>
    /// The wand mesh effects
    /// </summary>
    public PSMeshRendererUpdater[] wandMeshEffects;
    /// <summary>
    /// The ability fail effect
    /// </summary>
    public GameObject failEffect;
    /// <summary>
    /// Has the weapon been cleared of effects.
    /// </summary>
    private bool clear;
    /// <summary>
    /// Rift input reference
    /// </summary>
    public RiftInput rInput;
    /// <summary>
    /// The weapon reference
    /// </summary>
    private Weapon weapon;
    /// <summary>
    /// An effect for the tip of the weapon, if used for this ability.
    /// </summary>
    GameObject sEffect = null;
    /// <summary>
    /// The base weapon material.
    /// </summary>
    public Material baseMat;
    /// <summary>
    /// Is the player currently able to cast.
    /// </summary>
    public bool casting = false;
    /// <summary>
    /// The current element type index
    /// </summary>
    public int school = 0;

    /// <summary>
    /// The neutral bullet to shoot if no cast has been made
    /// </summary>
    public GameObject neutralBullet;

    /// <summary>
    /// The start position
    /// </summary>
    private Vector3 startPos ;
    /// <summary>
    /// The teleporting
    /// </summary>
    bool teleporting;
    /// <summary>
    /// The tele portal
    /// </summary>
    Portal telePortal;
    /// <summary>
    /// The last portal
    /// </summary>
    Portal lastPortal;
    /// <summary>
    /// The start portal
    /// </summary>
    public Portal startPortal;
    /// <summary>
    /// The current portal
    /// </summary>
    Portal currentPortal;

    /// <summary>
    /// The player view
    /// </summary>
    public PlayerView playerView;

    /// <summary>
    /// The able to fire
    /// </summary>
    public bool ableToFire = true;

    /// <summary>
    /// The canvas group
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// The fade
    /// </summary>
    private Valve.VR.SteamVR_Fade fade;

    /// <summary>
    /// The skill type (skill 1 , 2 or 3)
    /// </summary>
    private int skillType = 0;

    /// <summary>
    /// The element type of the player
    /// </summary>
    public ElementType.Type elementType;

    /// <summary>
    /// The damage modifier for abilities
    /// </summary>
    public int damageModifier = 0;

    /// <summary>
    /// Colours to use for dead vs alive player
    /// </summary>
    public Color alive, dead;
    /// <summary>
    /// The health values for the player
    /// </summary>
    public float maxHealth, currentHealth;

    /// <summary>
    /// The shield power values for the player
    /// </summary>
    public int shieldPower, maxshieldPower;
    /// <summary>
    /// Is the shield active
    /// </summary>
    public bool shieldActive;
    /// <summary>
    /// The shield text element (third party)
    /// </summary>
    public TextMeshProUGUI shieldText;

    /// <summary>
    /// The score text element (third party)
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// The tornado object
    /// </summary>
    public GameObject tornado;

    /// <summary>
    /// The player score
    /// </summary>
    public int score;

    /// <summary>
    /// The text elements for the skills (third party)
    /// </summary>
    public TextMeshProUGUI skill1Text, skill2Text, skill3Text;
    /// <summary>
    /// The image elements for the skills
    /// </summary>
    public Image skill1Image, skill2Image, skill3Image;

    /// <summary>
    /// The player health bar image
    /// </summary>
    private Image playerHealthBar;
    /// <summary>
    /// The UI text classes for each element type
    /// </summary>
    public SchoolTexts redTexts, blueTexts, purpleTexts;
    /// <summary>
    /// The available abilities
    /// </summary>
    List<Ability> abilities;
    /// <summary>
    /// All element type abilities.
    /// </summary>
    Ability Skill1,Skill2,Skill3,RedSkill1,RedSkill2,RedSkill3,PurpleSkill1,PurpleSkill2,PurpleSkill3,BlueSkill1,BlueSkill2,BlueSkill3;

    /// <summary>
    /// Class Ability.
    /// </summary>
    class Ability
    {
        /// <summary>
        /// The cooldown
        /// </summary>
        public int cooldown;
        /// <summary>
        /// The skill name
        /// </summary>
        public String skillName;
        /// <summary>
        /// Available or not.
        /// </summary>
        public bool available;
        /// <summary>
        /// The current cooldown time.
        /// </summary>
        public int currentcooldowntime;
        /// <summary>
        /// Ability level
        /// </summary>
        public int level;
        /// <summary>
        /// Current and max exp
        /// </summary>
        public float exp, maxExp;
        /// <summary>
        /// The base damage
        /// </summary>
        public int baseDamage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ability"/> class.
        /// </summary>
        /// <param name="cooldown">The cooldown.</param>
        /// <param name="skillName">Name of the skill.</param>
        /// <param name="maxExp">The maximum exp.</param>
        /// <param name="damage">The damage.</param>
        public Ability(int cooldown, String skillName, float maxExp, int damage)
        {
            this.cooldown = cooldown;
            this.skillName = skillName;
            this.baseDamage = damage;
            level = 1;

        }

        /// <summary>
        /// Adds exp. Levels up if max exp reached.
        /// </summary>
        /// <param name="e">The e.</param>
        public void addExp(float e)
        {
            exp += e;
            if(exp >= maxExp)
            {
                float tmp = exp - maxExp;

                level++;
                exp = tmp;
            }
        }

     
    }
    /// <summary>
    /// The player hit box.
    /// </summary>
    public GameObject hitBox, followHead;
    /// <summary>
    /// The spawn manager reference
    /// </summary>
    public SpawnManager spawnManager;

    /// <summary>
    /// Starts this instance.
    /// Sets all abilities and their UI elements as well as all character base variables such as current portal and health.
    /// </summary>
    void Start () {
        scoreText = null;
        Skill1 = new Ability(60, "Tornado", 1000, 2); //0 in list
        Skill2 = new Ability(30, "Placeholder2", 1000, 3);
        Skill3 = new Ability(30, "Placeholder3", 1000, 3);
        abilities = new List<Ability>();
        abilities.Add(Skill1);
        abilities.Add(Skill2);
        abilities.Add(Skill3);

        //Red School
        RedSkill1 = new Ability(0, "RedBeam", 1000, 2);
        RedSkill2 = new Ability(25, "PlaceholderRed2", 1000, 100);
        RedSkill3 = new Ability(10, "PlaceholderRed3", 1000, 2);

        //Purple
        PurpleSkill1 = new Ability(0, "PurpleBeam", 1000, 2);
        PurpleSkill2 = new Ability(25, "PlaceholderPurple2", 1000, 100);
        PurpleSkill3 = new Ability(10, "PlaceholderPurple3", 1000, 2);

        //Blue
        BlueSkill1 = new Ability(0, "BlueBeam", 1000, 2);
        BlueSkill2 = new Ability(25, "PlaceholderBlue2", 1000, 100);
        BlueSkill3 = new Ability(10, "PlaceholderBlue3", 1000, 2);

        abilities.Add(RedSkill1);
        abilities.Add(RedSkill2);
        abilities.Add(RedSkill3);

        abilities.Add(BlueSkill1);
        abilities.Add(BlueSkill2);
        abilities.Add(BlueSkill3);

        abilities.Add(PurpleSkill1);
        abilities.Add(PurpleSkill2);
        abilities.Add(PurpleSkill3);

        score = 0;
        
        maxshieldPower = 100;
        shieldPower = maxshieldPower;
        
        shieldText.text = (((float)shieldPower/(float)maxshieldPower)*100.00).ToString() + " %";
        StartCoroutine(PowerShield());
        maxHealth = 100;
        currentHealth = maxHealth;

        vrPlayer = this.GetComponent<Valve.VR.InteractionSystem.Player>();
        playerView = this.GetComponentInChildren<PlayerView>();
       
        transform.position = startPortal.spawnPos.position;
        currentPortal = startPortal;
        lastPortal = startPortal;

        startPortal.gameObject.SetActive(false);
        fade = GetComponentInChildren<Valve.VR.SteamVR_Fade>();
  

        //scoreText.text = score.ToString();
        StartCoroutine(ScoreTally());
        StartCoroutine(Cooldowns());
        
	}

    /// <summary>
    /// Ran every 1 second, decreases the cooldown on unavailable skills until their cooldown is 0, when they are made available again.
    /// Handles UI to reflect cooldowns.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator Cooldowns()
    {
        for(; ; )
        {
            yield return new WaitForSecondsRealtime(1);
            foreach(Ability s in abilities)
            {
                if (!s.available)
                {
                    if (s.currentcooldowntime <= 0)
                    {
                        s.available = true;
  
                        switch (s.skillName)
                        {
                            case "Tornado":
                                skill1Text.enabled = false;

                                break;

                            case "PlaceholderBlue2":
                                blueTexts.Skill2Cooldown.enabled = false;
                                break;

                            case "PlaceholderRed2":
                                redTexts.Skill2Cooldown.enabled = false;
                                break;

                            case "PlaceholderPurple2":
                                purpleTexts.Skill2Cooldown.enabled = false;
                                break;

                            case "PlaceholderBlue3":
                                blueTexts.Skill3Cooldown.enabled = false;
                                break;

                            case "PlaceholderRed3":
                                redTexts.Skill3Cooldown.enabled = false;
                                break;

                            case "PlaceholderPurple3":
                                purpleTexts.Skill3Cooldown.enabled = false;
                                break;
                        }

                    }
                    else
                    {

                        float cooldowntime = s.cooldown;
                        float cooldownremainingtime = s.currentcooldowntime;

                        switch (s.skillName)
                        {
                            case "Tornado":
                                
                                skill1Text.enabled = true;
                                skill1Image.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))));
                                skill1Text.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderBlue2":
                                blueTexts.Skill2Cooldown.enabled = true;
                                blueTexts.Skill2.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))));
                                blueTexts.Skill2Cooldown.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderRed2":
                                redTexts.Skill2Cooldown.enabled = true;
                                redTexts.Skill2.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))));
                                redTexts.Skill2Cooldown.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderPurple2":
                                purpleTexts.Skill2Cooldown.enabled = true;
                                purpleTexts.Skill2.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))));
                                purpleTexts.Skill2Cooldown.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderBlue3":
                                blueTexts.Skill3Cooldown.enabled = true;
                                blueTexts.Skill3.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))) );
                                blueTexts.Skill3Cooldown.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderRed3":
                                redTexts.Skill3Cooldown.enabled = true;
                                redTexts.Skill3.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))) );
                                redTexts.Skill3Cooldown.text = s.currentcooldowntime.ToString();
                                break;

                            case "PlaceholderPurple3":
                                purpleTexts.Skill3Cooldown.enabled = true;
                                purpleTexts.Skill3.color = new Color(225, 225, 225, ((1 - (cooldownremainingtime / cooldowntime))) );
                                purpleTexts.Skill3Cooldown.text = s.currentcooldowntime.ToString();
                                break;
                        }

                        s.currentcooldowntime--;
                    }

                }
            }
        }
    }

    /// <summary>
    /// Calculates the damage of an ability.
    /// </summary>
    /// <param name="abilityName">Name of the ability.</param>
    /// <returns>System.Int32.</returns>
    public int calcDamage(String abilityName)
    {
        //float baseModifier = 1f;

        
        foreach(Ability a in abilities)
        {
            if(a.skillName == abilityName)
            {
                print("DAMAGE CALC : " + a.skillName + " > CURRENT EXP < " + a.exp + " < CURRENT LEVEL > " + a.level);
                return Mathf.RoundToInt((a.baseDamage * (a.level)) *( ((currentScore)/2)/2));
            }
        }
        return 0;

    }

    /// <summary>
    /// Teleports to the looked at portal in the player view.
    /// </summary>
    public void Teleport()
    {
        if (playerView.getPortal())
        {
            lastPortal.transform.gameObject.GetComponent<BoxCollider>().enabled = true;
            if (!lastPortal)
            {
                startPos = transform.position;
                
            }
            else
            {
                startPos = lastPortal.spawnPos.transform.position;
                
            }
            lastPortal = playerView.getPortal();

            telePortal = playerView.getPortal();

            
            teleporting = true;
            fade.OnStartFade(Color.black, 0.6f, true);
            
        }

    }

    /// <summary>
    /// Adds score.
    /// </summary>
    /// <param name="s">The s.</param>
    public void AddScore(int s)
    {
        if (s < 0) return;
        score += s;
    }

    /// <summary>
    /// Manages displaying the score on the UI staggered.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator ScoreTally()
    {
        for(; ; )
        {
            if (scoreText != null) {
                if (int.Parse(scoreText.text) < score)
                {

                    scoreText.text = (int.Parse(scoreText.text) + 1).ToString();
                }
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    /// <summary>
    /// Ran once a second, checks for teleport input.
    /// Handles health Ui image.
    /// Handles teleport movement.
    /// </summary>
    void Update () {
        if (weapon) { weapon.modeText.text = elementType.ToString(); }
       
        if(!gameStarted && weapon && shield)
        {
            gameStarted = true;
            spawnManager.StartGame();
        }
        var healthProportion = (currentHealth / maxHealth);
  
        if (teleporting)
        {
            if (telePortal.levelPortal) { telePortal = telePortal.nextLevelPortal; spawnManager.LevelTrigger(); }

            telePortal.transform.gameObject.GetComponent<BoxCollider>().enabled = false;
            currentPortal.transform.gameObject.SetActive(true);
            currentPortal.childRotationTransform.gameObject.SetActive(true);


            transform.position = Vector3.Lerp(transform.position, telePortal.spawnPos.transform.position, 2.5f * Time.deltaTime);
            if (transform.position.magnitude > telePortal.spawnPos.transform.position.magnitude)
            {
              
                if ((transform.position - telePortal.spawnPos.transform.position).magnitude <= 0.1f)
                {
                  // transform.position = telePortal.spawnPos.transform.position;
                 
                    teleporting = false;
                    currentPortal.transform.gameObject.SetActive(false);

                }
                if ((transform.position - telePortal.spawnPos.transform.position).magnitude <= 2f)
                {
                    currentPortal = telePortal;
                    currentPortal.childRotationTransform.gameObject.SetActive(false);
                    Color trans = new Color(0, 0, 0, 0);
                    fade.OnStartFade(trans, 0.5f, false);
                    transform.rotation = currentPortal.spawnPos.rotation;
                }

            }
            else
            {
                if ((telePortal.spawnPos.transform.position- transform.position ).magnitude <= 0.1f)
                {
                   //transform.position = telePortal.spawnPos.transform.position;

                    teleporting = false;
                    currentPortal.transform.gameObject.SetActive(false);
                }
                if ((telePortal.spawnPos.transform.position - transform.position).magnitude <= 2f)
                {
                    currentPortal = telePortal;
                    currentPortal.childRotationTransform.gameObject.SetActive(false);
                    Color trans = new Color(0,0,0,0);
                    fade.OnStartFade(trans, 0.6f, false);
                    transform.rotation = currentPortal.spawnPos.rotation;

                }

            }
        }

        if (rInput.shield != null)
        {
            if(playerHealthBar == null)
            {
                playerHealthBar = rInput.shield.GetComponent<Shield>().playerHealthBar;
            }
            if(scoreText == null)
            {
                scoreText = rInput.shield.GetComponent<Shield>().score;
            }
            playerHealthBar.fillAmount = currentHealth / maxHealth;
        }

        hitBox.transform.position = followHead.transform.position;
        
	}

    /// <summary>
    /// Reports any hit to the player or their shield.
    /// </summary>
    /// <param name="spellID">The spell identifier.</param>
    /// <param name="hit">The hit.</param>
    public void ReportHit(int spellID, RaycastHit hit)
    {
        if(hit.transform.tag == "Shield")
        {
            print("BLOCKED");

        }

        if(hit.transform.tag == "Finish" || hit.transform.tag == "Player")
        {
            print("HIT PLAYER");
            currentHealth -= 5;
        }


    }

    /// <summary>
    /// Checks which beam to cast, and informs the Weapon script.
    /// </summary>
    /// <param name="t">if set to <c>true</c> [t].</param>
    public void Beam(bool t)
    {
        if (ableToFire && casting)
        {
            if (t)
            {
                if (skillType == 1)
                {
                    String schoolname = "";

                    switch (school)
                    {
                        case 0:
                            schoolname = "Red";
                            break;

                        case 1:
                            schoolname = "Purple";
                            break;

                        case 2:
                            schoolname = "Blue";
                            break;

                    }
                    weapon.Beam(schoolname);
                }
            }
            else
            {
                if (weapon)
                    weapon.EndBeam();
            }
        }
        if (!t && weapon)
            weapon.EndBeam();
    }

    /// <summary>
    /// If the player is able to shoot, this method handles retreiving which ability to cast.
    /// </summary>
    public void Fire()
    {
 
        if (casting)
        {

            if (rInput.drawing) return;
            //GameObject proj = null;
            GameObject[] skillSet = null;
            switch (school)
            {
                case 0:
                    skillSet = LightningSkillSet;
                    break;
                case 1:
                    skillSet = ForceSkillSet;
                    break;
                case 2:
                    skillSet = WaterSkillSet;
                    break;
                default:
                    break;
            }
            switch (skillType)
            {
                case 1:

                    String schoolname = "";

                    switch (school)
                    {
                        case 0:
                            schoolname = "Red";
                            break;

                        case 1:
                            schoolname = "Purple";
                            break;

                        case 2:
                            schoolname = "Blue";
                            break;

                    }
                    
                    weapon.Beam(schoolname);




                    break;


                case 2:

                    string sskillName = "";
                    switch (elementType)
                    {
                        case ElementType.Type.Blue:
                            sskillName = "PlaceholderBlue2";
                            break;
                        case ElementType.Type.Purple:
                            sskillName = "PlaceholderPurple2";
                            break;
                        case ElementType.Type.Red:
                            sskillName = "PlaceholderRed2";
                            break;
                    }

                    foreach (Ability s in abilities)
                    {
                        if (s.skillName == sskillName)
                        {
                            if (s.available)
                            {
                                s.available = false;
                                s.currentcooldowntime = s.cooldown;
                                ableToFire = true;
                                StartCoroutine(abilityScore(s));
                            }
                            else
                            {
                                ableToFire = false;
                            }
                        }
                    }

                    if (target && ableToFire)
                    {
                        if (skillSet[skillType - 1].GetComponent<TargetBlast>().useCustomObject)
                        {
                            target.GetComponent<Enemy>().spawnObject(skillSet[skillType - 1].GetComponent<TargetBlast>().customObjectID, true);
                            HitNearby(skillSet[skillType - 1], target, 3);
                            ClearSpell();
                            clearRenders();
                        }
                        else
                        {
                           // GameObject blast = GameObject.Instantiate(skillSet[skillType - 1], target.transform.position, target.transform.rotation);
                           // blast.GetComponent<TargetBlast>().target = target;
                            //blast.GetComponent<TargetBlast>().playerControlled = true;
                            print("NON CUSTOM OBJECT");
                        }
                        ableToFire = false;

                    }

                    break;




                case 3:
                    print("three");
                    string skillName = "";
                    switch (elementType)
                    {
                        case ElementType.Type.Blue:
                             skillName = "PlaceholderBlue3";
                            break;
                        case ElementType.Type.Purple:
                             skillName = "PlaceholderPurple3";
                            break;
                        case ElementType.Type.Red:
                             skillName = "PlaceholderRed3";
                            break;
                    }

                    foreach(Ability s in abilities)
                    {
                        if(s.skillName == skillName)
                        {
                            if (s.available)
                            {
                                s.available = false;
                                s.currentcooldowntime = s.cooldown;
                                StartCoroutine(abilityScore(s));
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            
            
            
        }
    }

    /// <summary>
    /// Adds exp to abilities.
    /// </summary>
    /// <param name="a">a.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator abilityScore(Ability a)
    {

        float tmp = score;
        yield return new WaitForSeconds(10);
        a.addExp((score - tmp) / 10);
        print("added exp : " + (score - tmp) / 10 + " to : " + a.skillName);
        
    }

    /// <summary>
    /// Hits a nearby enemy. Used for blast skills.
    /// </summary>
    /// <param name="blast">The blast.</param>
    /// <param name="target">The target.</param>
    /// <param name="jumps">The jumps.</param>
    public void HitNearby(GameObject blast, GameObject target, int jumps)
    {

            StartCoroutine(Hit(blast, target, jumps));
    
    }

    /// <summary>
    /// Use the tornado skill.
    /// </summary>
    public void TornadoSkill()
    {
        if (Skill1.available)
        {
            Skill1.currentcooldowntime = Skill1.cooldown;
            Skill1.available = false;
            tornado.SetActive(true);
            tornado.GetComponent<Tornado>().time = 20;
            tornado.GetComponent<Tornado>().Summon();
            ClearSpell();
            clearRenders();
        }
        else
        {
            ClearSpell();
            clearRenders();
        }
    }

    /// <summary>
    /// Calculates enemy hit for the specified blast.
    /// </summary>
    /// <param name="blast">The blast.</param>
    /// <param name="target">The target.</param>
    /// <param name="jumps">The jumps.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator Hit(GameObject blast, GameObject target, int jumps)
    {

        jumps -= 1;

        yield return new WaitForSeconds(2f);
        
        GameObject newTarget = target.GetComponent<Enemy>().RandomEnemy();

        if (newTarget && newTarget != target)
        {
            newTarget.GetComponent<Enemy>().spawnObject(blast.GetComponent<TargetBlast>().customObjectID, true);

        }
        if (jumps != 0)
        {
            HitNearby(blast, newTarget, jumps);
        }


    }


    /// <summary>
    /// Takes the damage passed.
    /// </summary>
    /// <param name="damage">The damage.</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

    }

    /// <summary>
    /// Fires the specified bullet.
    /// </summary>
    /// <param name="bullet">if set to <c>true</c> [bullet].</param>
    public void Fire(bool bullet)
    {
        //for firing bullets when no spell is chosen
        if (ableToFire)
        {
            if (rInput.drawing) return;

            if (!weapon) weapon = rInput.weapon;
            

            GameObject proj = GameObject.Instantiate(neutralBullet, weapon.particleComplete.transform.position, weapon.particleComplete.transform.rotation);
            proj.GetComponent<Projectile>().damage += damageModifier;
            StartCoroutine(FireTime(0.1f));

            ableToFire = false;
        }
    }

    /// <summary>
    /// After a cast, waits for time before the player can cast again.
    /// </summary>
    /// <param name="time">The time.</param>
    /// <returns>IEnumerator.</returns>
    IEnumerator FireTime(float time)
    {
        yield return new WaitForSeconds(time);
        ableToFire = true;
    }
    /// <summary>
    /// Clears all spells and weapon effects.
    /// </summary>
    public void ClearSpell()
    {
        Beam(false);
        casting = false;
        if (clear) return;
        clearRenders();
        if (!weapon) weapon = rInput.weapon;

        clear = true;
    }


    /// <summary>
    /// Clears all weapon renderers.
    /// </summary>
    void clearRenders()
    {
        
        try
        {
            if (sEffect) Destroy(sEffect);

            for (int i = 0; i < weapon.GetComponentInChildren<MeshRenderer>().materials.Length; i++)
            {
                if (i == 0)
                {
                    weapon.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;
                }
                else
                {
                    Destroy(weapon.GetComponentInChildren<MeshRenderer>().materials[i]);
                }
            }
            weapon.UpdateMesh(elementType, false);
        }
        catch(Exception e)
        {

        }

        
    }

    /// <summary>
    /// Shields up.
    /// </summary>
    /// <param name="t">if set to <c>true</c> [t].</param>
    public void ShieldUp(bool t)
    {
        if (t)
        {
            if (shieldPower > 0)
            {
                shieldActive = true;
                shield.SetActive(t);
            }

        }
        else
        {
            shieldActive = false;
            shield.SetActive(t);
        }

    }

    /// <summary>
    /// Handles shield power.
    /// </summary>
    /// <returns>IEnumerator.</returns>
    IEnumerator PowerShield()
    {
        for (; ;){

            shieldText.text = (((float)shieldPower / (float)maxshieldPower) * 100.00).ToString() + " %";
            if (shieldActive)
            {
                if (shieldPower > 0)
                {
                    shieldPower--;
                }
                else
                {
                    ShieldUp(false);
                }
                
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                yield return new WaitForSeconds(1f);

                if (shieldPower < maxshieldPower && !shieldActive)
                {
                    shieldPower++;
                }
            }

        }

    }

    /// <summary>
    /// This method is called if the ability has failed for what ever reason. Spawns the fail effect.
    /// </summary>
    public void CastSpell()
    {
        try
        {
            weapon.transform.GetComponentInChildren<MeshRenderer>().material = baseMat;
            weapon.transform.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;

        }catch(Exception e)
        {
            print("failed for some reason");
        }

        clear = false;
        if (sEffect) Destroy(sEffect);
        if (!weapon) weapon = rInput.weapon;
        weapon.beaming = false;
        sEffect = GameObject.Instantiate(failEffect, weapon.particleComplete.transform);
        print("Too small");
        casting = false;
        Vector3 resetPos = new Vector3(0, 0, 0);
        sEffect.transform.localPosition = resetPos;
    }
    /// <summary>
    /// Casts the ability if it has succeeded. This method matches the gesture to the skill and does the appropriate action.
    /// </summary>
    /// <param name="result">The result.</param>
    public void CastSpell(DollarRecognizer.Result result)
    {

        bool t = true;
        clear = false;
        if (!weapon) weapon = rInput.weapon;
        weapon.beaming = false;
        print(result.Match.Name.Substring(0,1));

        float distfromOne = 1 - result.Score;


        currentScore = (int)Mathf.Round(distfromOne * 10);

        print("current score : " + currentScore);

        print("Closest result : " + result.Match.Name);

        if (currentScore >= 5)
        {
            switch (result.Match.Name.Substring(0, 2))
            {
                case "ON":

                    skillType = 1;
                    print("Skill 1 (Beam)");
                    weapon.beamSeconds = 20f * (1f + ((currentScore - 5) / 10f));
                    weapon.beaming = true;
                    casting = true;

                    string aName = "";
                    switch (elementType)
                    {
                        case ElementType.Type.Blue:
                            aName = "BlueBeam";
                            break;
                        case ElementType.Type.Purple:
                            aName = "PurpleBeam";
                            break;
                        case ElementType.Type.Red:
                            aName = "RedBeam";
                            break;
                    }
                    foreach (Ability s in abilities)
                    {
                        if (s.skillName == aName)
                        {
                            StartCoroutine(abilityScore(s));
                        }
                    }

                    break;

                case "TW":

                    skillType = 2;
                    print("SKill 2");
                    casting = true;
                    break;

                case "TH":

                    skillType = 3;
                    print("Skill 3");
                    casting = true;
                    
                    break;

                case "VV":
                    print("GOT V!");

                    TornadoSkill();

                    break;

                case "GG":
                    print("GOT G!");

                    break;

                case "MM":
                    print("GOT M!");
                    break;

                default:

                    t = false;
                    print("Unrecognised");
                    break;
            }

            weapon.UpdateMesh(elementType, true);
            
        }
        else
        {

            weapon.UpdateMesh(elementType, false);

            sEffect = GameObject.Instantiate(failEffect, weapon.particleComplete.transform);
            print("Score too low");
        }

        if (!t) return;
        Vector3 resetPos = new Vector3(0,0,0);

    }



    /// <summary>
    /// If the time on the beam seconds has finished, clear all abilities and weapon.
    /// </summary>
    public void BeamTimed()
    {
        ClearSpell();
        CastSpell();
        weapon.beaming = false;

    }

    /// <summary>
    /// Cycles through the element types. Referred to as "schools"
    /// </summary>
    public void IncrementSkillMode()
    {
        currentScore = 0;
        casting = false;

       
        clearRenders();

        if (school < 2)
        {
            school++;
        }
        else
        {
            school = 0;
        }

        AssignElement();
    }

    /// <summary>
    /// Assigns the elementtype to the weapon mesh and effects.
    /// </summary>
    public void AssignElement()
    {
        redTexts.gameObject.SetActive(false);
        blueTexts.gameObject.SetActive(false);
        purpleTexts.gameObject.SetActive(false);

        weapon = rInput.weapon;
        if (weapon)
        {
            switch (school)
            {
                case 0:
                    weapon.ChangeMesh("Red");
                    print("Red");  //Red
                    elementType = ElementType.Type.Red;
                    redTexts.gameObject.SetActive(true);
                    break;
                case 1:
                    weapon.ChangeMesh("Purple");
                    print("Purple");  //Purple
                    elementType = ElementType.Type.Purple;
                    purpleTexts.gameObject.SetActive(true);
                    break;
                case 2:
                    weapon.ChangeMesh("Blue");
                    print("Blue");  //Blue
                    elementType = ElementType.Type.Blue;
                    blueTexts.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
