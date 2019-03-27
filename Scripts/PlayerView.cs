using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {

    public Portal lookingAt;

    public List<Portal> lookingAts;

    public bool firstTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();
        lookingAts.Add(other.transform.GetComponent<Portal>());

        lookingAt.LookedAt();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();
        lookingAts.Remove(other.transform.GetComponent<Portal>());
        lookingAt.LookedAway();
        lookingAt = null;
    }

    public Portal getPortal()
    {
        if (lookingAts.Count == 0) return null;

        return lookingAts[lookingAts.Count-1];
       // return lookingAt;
    }

}
