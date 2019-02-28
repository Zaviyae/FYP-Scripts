using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpThrow : MonoBehaviour {

    public float yAxis;
    public GameObject[] elemental;
    public GameObject sphere;
    public bool thrown;
    public int elementalID;
    public bool random,set;
    public bool objFloat;
    void Start() {
        
    }

    void Update() {
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
                    elemental[Random.Range(0, elemental.Length)].SetActive(true);
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
}
