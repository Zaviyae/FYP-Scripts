﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    public bool effectCollisionBox;
    public GameObject tBlastObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            if (!effectCollisionBox)
            {
                GetComponentInParent<Player>().TakeDamage(10f);
                other.GetComponent<Enemy>().Explode();
            }
            else
            {
                    float fDamage = tBlastObject.GetComponent<TargetBlast>().damage * ElementType.getDamageModifier(tBlastObject.GetComponent<TargetBlast>().type, other.GetComponent<Enemy>().elementType);
                    print("AOE damage : " + Mathf.RoundToInt(fDamage));
                    other.GetComponent<Enemy>().TakeDamage(Mathf.RoundToInt(fDamage)); 
            }
        }
    }
}
