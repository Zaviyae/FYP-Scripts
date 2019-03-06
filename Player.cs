using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour {

    private Valve.VR.InteractionSystem.Player vrPlayer;

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
    public int skillMode = 0; 

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
                weapon.Beam();
            }
        }
        else
        {
            weapon.EndBeam();
        }

    }

    public void Fire()
    {
 
        if (ableToFire)
        {

            if (rInput.drawing) return;
            GameObject proj = null;
            GameObject[] skillSet = null;
            switch (skillMode)
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
                    weapon.Beam();

                    break;
                case 2:
                    if (target)
                    {
                        if (skillSet[skillType - 1].GetComponent<TargetBlast>().useCustomObject)
                        {
                            target.GetComponent<Enemy>().spawnObject(skillSet[skillType - 1].GetComponent<TargetBlast>().customObjectID);
                            
                        }
                        else
                        {
                            GameObject blast = GameObject.Instantiate(skillSet[skillType - 1], target.transform.position, target.transform.rotation);
                            blast.GetComponent<TargetBlast>().target = target;
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

        print("Actual score = " + result.Score);
        float distfromOne = 1 - result.Score;

        print("Score : " + Mathf.Round(distfromOne * 10));
        print("Closest result : " + result.Match.Name);
        if (distfromOne * 10 >= 6)
        {
            switch (result.Match.Name.Substring(0, 2))
            {
                case "ON":
                    //sEffect = GameObject.Instantiate(skillEffectsM[skillMode], weapon.particleComplete.transform);
                    skillType = 1;
                    print("Skill 1 (Basic Projectile)");
                    casting = true;
                    break;
                case "TW":
                    //sEffect = GameObject.Instantiate(skillEffectsL[skillMode], weapon.particleComplete.transform);
                    skillType = 2;
                    print("SKill 2");
                    casting = true;
                    break;
                case "TH":
                    //sEffect = GameObject.Instantiate(skillEffectsV[skillMode], weapon.particleComplete.transform);
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

            
            wandMeshEffects[skillMode].transform.gameObject.SetActive(true);

            wandMeshEffects[skillMode].GetComponent<PSMeshRendererUpdater>().UpdateMeshEffect();
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
       // sEffect.transform.localPosition = resetPos;



    }

    public void IncrementSkillMode()
    {
        casting = false;
        try
        {
            weapon.transform.GetComponentInChildren<MeshRenderer>().material = baseMat;
            weapon.transform.GetComponentInChildren<MeshRenderer>().materials[0] = baseMat;
        }
        catch (Exception e)
        {
            print("failed for some reason");
        }
        clearRenders();

        if (skillMode < 2)
        {
            skillMode++;
        }
        else
        {
            skillMode = 0;
        }

        AssignElement();
    }

    void AssignElement()
    {
        switch (skillMode)
        {
            case 0:
                print("Lightning");
                elementType = ElementType.Type.Lightning;
                break;
            case 1:
                print("Force");
                elementType = ElementType.Type.Force;
                break;
            case 2:
                print("Water");
                elementType = ElementType.Type.Water;
                break;
        }
    }
}
