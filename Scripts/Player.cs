using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {

    private bool gameStarted;

    private Valve.VR.InteractionSystem.Player vrPlayer;

    public int currentScore = 0;
    public GameObject shield;

    public GameObject target;

    public GameObject[] LightningSkillSet;
    public GameObject[] WaterSkillSet;
    public GameObject[] ForceSkillSet;

    public Dictionary<GameObject, GameObject> skillMap;


    private GameObject projectile;
    public PSMeshRendererUpdater[] wandMeshEffects;
    public GameObject failEffect;
    private bool clear;
    public RiftInput rInput;
    private Weapon weapon;
    GameObject sEffect = null;
    public Material baseMat;
    public bool casting = false;
    public int school = 0; 

    public bool canShootBullets = true;
    public GameObject neutralBullet;

    private Vector3 startPos ;
    bool teleporting;
    Portal telePortal;
    Portal lastPortal;
    public Portal startPortal;
    Portal currentPortal;

    public PlayerView playerView;

    public bool ableToFire = true;

    private CanvasGroup canvasGroup;

    private Valve.VR.SteamVR_Fade fade;

    private int skillType = 0;

    public ElementType.Type elementType;
    public int damageModifier = 0;

    public Color alive, dead;
    public float maxHealth, currentHealth;

    public int shieldPower, maxshieldPower;
    public bool shieldActive;
    public TextMeshProUGUI shieldText;

    public TextMeshProUGUI scoreText;

    public GameObject tornado;
    public int score;
    
    public TextMeshProUGUI skill1Text, skill2Text, skill3Text;
    public Image skill1Image, skill2Image, skill3Image;

    private Image playerHealthBar;
    public SchoolTexts redTexts, blueTexts, purpleTexts;
    List<Ability> abilities;
    Ability Skill1,Skill2,Skill3,RedSkill1,RedSkill2,RedSkill3,PurpleSkill1,PurpleSkill2,PurpleSkill3,BlueSkill1,BlueSkill2,BlueSkill3;

    class Ability
    {
        public int cooldown;
        public String skillName;
        public bool available;
        public int currentcooldowntime;
        public int level;
        public float exp, maxExp;
        public int baseDamage;

        public Ability(int cooldown, String skillName, float maxExp, int damage)
        {
            this.cooldown = cooldown;
            this.skillName = skillName;
            this.baseDamage = damage;
            level = 1;

        }

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
    public GameObject hitBox, followHead;
    public SpawnManager spawnManager;

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

        /*
        switch (num)
        {
            case 1:       
                return Mathf.RoundToInt(GlobalVariables.BASE_ONE_DAMAGE + (currentScore - 5));
                
            case 2:

                return Mathf.RoundToInt(GlobalVariables.BASE_TWO_DAMAGE * (baseModifier + ((currentScore - 5) / 10f)));
                
            case 3:

                return Mathf.RoundToInt(GlobalVariables.BASE_THREE_DAMAGE * (baseModifier + ((currentScore - 5) / 10f)));
                
            default:

                return 0;
        }
        */
    }

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

    public void AddScore(int s)
    {
        if (s < 0) return;
        score += s;
    }

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

    IEnumerator abilityScore(Ability a)
    {

        float tmp = score;
        yield return new WaitForSeconds(10);
        a.addExp((score - tmp) / 10);
        print("added exp : " + (score - tmp) / 10 + " to : " + a.skillName);
        
    }

    public void HitNearby(GameObject blast, GameObject target, int jumps)
    {

            StartCoroutine(Hit(blast, target, jumps));
    
    }

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
    

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

    }

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

    IEnumerator FireTime(float time)
    {
        yield return new WaitForSeconds(time);
        ableToFire = true;
    }
    public void ClearSpell()
    {
        Beam(false);
        casting = false;
        if (clear) return;
        clearRenders();
        if (!weapon) weapon = rInput.weapon;

        clear = true;
    }

    
    void clearRenders()
    {
        try
        {
            if (sEffect) Destroy(sEffect);
            foreach (PSMeshRendererUpdater g in wandMeshEffects)
            {
                g.transform.gameObject.SetActive(false);
            }

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
            
        }
        catch(Exception e)
        {

        }
    }
    
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
       // clearRenders();

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
    public void CastSpell(DollarRecognizer.Result result)
    {

        bool t = true;
        clear = false;
       // clearRenders();
        if (!weapon) weapon = rInput.weapon;
        weapon.beaming = false;
        print(result.Match.Name.Substring(0,1));

       // print("Actual score = " + result.Score);
        float distfromOne = 1 - result.Score;

       // print("Score : " + Mathf.Round(distfromOne * 10));

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
            try
            {
                weapon.transform.GetComponentInChildren<MeshRenderer>().material = baseMat;
                weapon.transform.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;
            }
            catch (Exception e)
            {
                print("failed for some reason");
            }

            
            wandMeshEffects[school].transform.gameObject.SetActive(true);

            wandMeshEffects[school].GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect();
        }
        else
        {
            try
            {
                weapon.transform.GetComponentInChildren<MeshRenderer>().material = baseMat;
                weapon.transform.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;
            }
            catch (Exception e)
            {
                print("failed for some reason");
            }
            foreach (PSMeshRendererUpdater g in wandMeshEffects)
            {
                g.transform.gameObject.SetActive(false);
            }

            sEffect = GameObject.Instantiate(failEffect, weapon.particleComplete.transform);
            print("Score too low");
        }

        if (!t) return;
        Vector3 resetPos = new Vector3(0,0,0);

    }

    

    public void BeamTimed()
    {
        ClearSpell();
        CastSpell();
        weapon.beaming = false;
        // casting = false;
    }

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
