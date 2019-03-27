using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Rigidbody rbody;
    SmartMissile sm;

    public GameObject explosionEffect;

    public ElementType.Type elementType;

    public int damage = 10;

    public float speed;
    public GameObject player;

    bool triggered;

    private RFX4_EffectSettings rfx;

  //  void Start () {
       // rbody = this.GetComponent<Rigidbody>();
     //   sm = this.GetComponent<SmartMissile>();
       // rbody.AddForce(transform.forward * 400f);

   //}

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        try
        {
            var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
            if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

            var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
            if (raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
            
            rfx = GetComponent<RFX4_EffectSettings>();
        }
        catch (Exception e)
        {

        }


    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            SlowMo();
        }
    }

    private void SlowMo()
    {
        if (!triggered)
        {
            triggered = true;
            if(rbody) rbody.velocity = (rbody.velocity * 0.2f);

            if (rfx)
            {
                rfx.Speed = (rfx.Speed * 0.5f);
            }
        }

    }

    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        if(e.HitGameObject.tag == "Enemy")
        {
            Enemy enemy = e.HitGameObject.transform.GetComponent<Enemy>();

            if (enemy)
            {
                float fDamage = damage * ElementType.getDamageModifier(elementType, enemy.elementType);
                print("Projectile damage to enemy : " + Mathf.RoundToInt(fDamage));
                enemy.TakeDamage(player.GetComponent<Player>().calcDamage(1), elementType);
            }
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            print("hit shield, bye.");
            Destroy(this.gameObject);
        }
    }
    public void Override(ElementType.Type elType)
    {
        elementType = elType;

        /*
            rbody = this.GetComponent<Rigidbody>();
            rbody.AddForce(transform.forward * speed);
        */

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.tag == "Player")
        {
            print("hit player!");
        }


    }

}
