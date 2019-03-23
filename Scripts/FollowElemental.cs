using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowElemental : MonoBehaviour {

    public GameObject playerView;

    // Update is called once per frame
    void Update () {
        //Quaternion newPos = new Quaternion(playerView.transform.rotation.x,transform.rotation.y,transform.rotation.z, transform.rotation.w);
     //   Quaternion newPos = new Quaternion(0, playerView.transform.rotation.eulerAngles.y, 0, 0);


      //  print(newPos.y);
       // transform.rotation = newPos;

        transform.rotation = Quaternion.Euler(transform.rotation.x, playerView.transform.rotation.eulerAngles.y, transform.rotation.z);
	}
}
