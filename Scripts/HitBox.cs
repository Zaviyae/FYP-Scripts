using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public bool effectCollisionBox;
    public GameObject tBlastObject;
    public Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (!effectCollisionBox)
            {
                GetComponentInParent<Player>().TakeDamage(10f);
                other.GetComponent<Enemy>().ReportDeath();
            }
            else
            {
                    float fDamage = tBlastObject.GetComponent<TargetBlast>().damage * ElementType.getDamageModifier(tBlastObject.GetComponent<TargetBlast>().type, other.GetComponent<Enemy>().elementType);
                    print("AOE damage : " + Mathf.RoundToInt(fDamage));


                print("DISABLE THIS.");
                  //  other.GetComponent<Enemy>().TakeDamage(player.calcDamage(2), tBlastObject.GetComponent<TargetBlast>().type); 
            }
        }
    }
}
