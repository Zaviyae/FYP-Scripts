using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Rigidbody rbody;
    SmartMissile sm;

    public GameObject explosionEffect;

    public ElementType.Type elementType;

    public int damage = 10;

  //  void Start () {
       // rbody = this.GetComponent<Rigidbody>();
     //   sm = this.GetComponent<SmartMissile>();
       // rbody.AddForce(transform.forward * 400f);

   // }

    void Start()
    {
        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

        var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if (raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
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
                enemy.TakeDamage(Mathf.RoundToInt(fDamage));
            }
        }
        Debug.Log(e.HitPoint); //a collision coordinates in world space
        Debug.Log(e.HitGameObject.name); //a collided gameobject
        Debug.Log(e.HitCollider.name); //a collided collider :)
    }


    public void Override(ElementType.Type elType)
    {
        elementType = elType;
        rbody = this.GetComponent<Rigidbody>();
        rbody.AddForce(transform.forward * 1400f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.transform.tag == "Ignore") return;

        Enemy enemy = collision.transform.GetComponent<Enemy>();

        if (enemy)
        {
            float fDamage = damage * ElementType.getDamageModifier(elementType, enemy.elementType);
            print("Projectile damage to enemy : " + Mathf.RoundToInt(fDamage));
            enemy.TakeDamage(Mathf.RoundToInt(fDamage));
        }

        //GameObject.Instantiate(explosionEffect, this.transform.position, this.transform.rotation);

        Destroy(this.gameObject);
        */
    }

}
