using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {

    private Portal lookingAt;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();

        lookingAt.LookedAt();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Portal") return;

        lookingAt = other.transform.GetComponent<Portal>();

        lookingAt.LookedAway();
        lookingAt = null;
    }

    public Portal getPortal()
    {
        return lookingAt;
    }

}
