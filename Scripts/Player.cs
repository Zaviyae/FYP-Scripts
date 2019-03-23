using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {

    private Valve.VR.InteractionSystem.Player vrPlayer;

    public int currentScore = 0;
    public GameObject shield;

    public GameObject target;

    public Elemental[] Elementals;
    public Elemental eFire, eWater, eForce;

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

    private ElementType.Type elementType;
    public int damageModifier = 0;

    public Color alive, dead;
    public float maxHealth, currentHealth;

    public int shieldPower, maxshieldPower;
    public bool shieldActive;
    public TextMeshProUGUI shieldText;
   
    void Start () {
        
        maxshieldPower = 100;
        shieldPower = maxshieldPower;
        
        shieldText.text = (((float)shieldPower/(float)maxshieldPower)*100.00).ToString() + " %";
        StartCoroutine(PowerShield());
        maxHealth = 100;
        currentHealth = maxHealth;

        wandMeshEffects = new PSMeshRendererUpdater[3];
        vrPlayer = this.GetComponent<Valve.VR.InteractionSystem.Player>();
        playerView = this.GetComponentInChildren<PlayerView>();
       
        transform.position = startPortal.spawnPos.position;
        currentPortal = startPortal;
        lastPortal = startPortal;

        startPortal.gameObject.SetActive(false);
        fade = GetComponentInChildren<Valve.VR.SteamVR_Fade>();
        AssignElement();
        
	}
	
    public int calcDamage(int num)
    {
        float baseModifier = 1f;

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


	void Update () {
        if (weapon) { weapon.modeText.text = elementType.ToString(); }
       
        var healthProportion = (currentHealth / maxHealth);
  
        if (teleporting)
        {
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
	}

    public void Beam(bool t)
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
            if(weapon)
                weapon.EndBeam();
            }

    }

    public void Fire()
    {
 
        if (ableToFire)
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

                    // proj = GameObject.Instantiate(skillSet[skillType -1], weapon.particleComplete.transform.position, weapon.particleComplete.transform.rotation);
                    // StartCoroutine(FireTime(0.3f));
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
                    if (target)
                    {
                        if (skillSet[skillType - 1].GetComponent<TargetBlast>().useCustomObject)
                        {
                            target.GetComponent<Enemy>().spawnObject(skillSet[skillType - 1].GetComponent<TargetBlast>().customObjectID, true);
                            
                        }
                        else
                        {
                            GameObject blast = GameObject.Instantiate(skillSet[skillType - 1], target.transform.position, target.transform.rotation);
                            blast.GetComponent<TargetBlast>().target = target;
                            blast.GetComponent<TargetBlast>().playerControlled = true;
                        }
                        ableToFire = false;
                        StartCoroutine(FireTime(1f));
                    }
                    break;
                default:
                    break;
            }
           // proj.GetComponent<Projectile>().damage += damageModifier;
            //proj.GetComponent<Projectile>().elementType = elementType;
            
            
            
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
        clearRenders();

        clear = false;
        if (sEffect) Destroy(sEffect);
        if (!weapon) weapon = rInput.weapon;

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
        clearRenders();
        if (!weapon) weapon = rInput.weapon;
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
                    casting = true;
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

    public void IncrementSkillMode()
    {
        currentScore = 0;
        casting = false;
        try
        {
           // weapon.transform.GetComponentInChildren<MeshRenderer>().material = baseMat;
           // weapon.transform.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;
        }
        catch (Exception e)
        {
            print("failed for some reason");
        }
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

    void AssignElement()
    {
        switch (school)
        {
            case 0:
                print("Lightning");  //Red
                elementType = ElementType.Type.Lightning;
                break;
            case 1:
                print("Force");  //Purple
                elementType = ElementType.Type.Force;
                break;
            case 2:
                print("Water");  //Blue
                elementType = ElementType.Type.Water;
                break;
        }
    }
}
