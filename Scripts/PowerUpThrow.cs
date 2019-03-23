using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpThrow : MonoBehaviour {

    public float yAxis;
    public GameObject[] elemental;
    public GameObject sphere;
    public bool thrown, shieldcontact;
    public int elementalID;
    public bool random,set;
    public bool objFloat;
    public Rigidbody rBody;
    void Start() {
        rBody = GetComponent<Rigidbody>();
    }

    void Update() {

        if (shieldcontact)
        {

        }


        if (thrown)
        {
            if (!set)
            {
                yAxis = transform.position.y;
                print(yAxis + "y axis");
                set = true;
            }

            if (yAxis <= transform.position.y)
            {
                yAxis = transform.position.y;
            }
            else
            {
                
                print(yAxis + " >< " + transform.position.y);
                print("PEAKED HEIGHT " + yAxis);
                //release elemental
                if (!random)
                {
                    elemental[elementalID].SetActive(true);
                }
                else
                {
                    int chosenOne = Random.Range(0, elemental.Length);

                    foreach(GameObject g in elemental)
                    {
                        if (elemental[chosenOne] == g)
                        {
                            g.SetActive(true);
                        }
                        else
                        {
                            Destroy(g);
                        }
                    }

                    
                }

                sphere.SetActive(false);

                transform.DetachChildren();
                Destroy(this.gameObject);
                
            }
        }
        if (objFloat)
        {
            transform.position += transform.forward * Time.deltaTime * 1.5f;

            if (transform.root.tag == "Player")
            {
                objFloat = false;
            }
        }
    }

    public void floating()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().useGravity = false;
        transform.LookAt(GameObject.FindGameObjectWithTag("PlayerTarget").transform);
        transform.localScale = new Vector3(2, 2, 2);
        objFloat = true;
    }
    public void threw(){

        thrown = true;
        }


    public void ShieldTouch()
    {
        GetComponent<Rigidbody>().useGravity = true;
        shieldcontact = true;
        objFloat = false;

        // EXPLODE
        thrown = true;
    }

    public void ShieldTouch(float x, Collision c, Transform trans)
    {
        GetComponent<Rigidbody>().useGravity = true;
        shieldcontact = true;
        objFloat = false;

  

      //  print("called");
        // force is how forcefully we will push the player away from the enemy.
       // float force = 3000f;

        // If the object we hit is the enemy

        // Calculate Angle Between the collision point and the player
     //   Vector3 dir = c.contacts[0].point + trans.position;
        // We then get the opposite (-Vector3) and normalize it
      //  dir = -dir.normalized;
        // And finally we add force in the direction of dir and multiply it by force. 
        // This will push back the player
      //  GetComponent<Rigidbody>().AddForce(dir * force);

    }
}
