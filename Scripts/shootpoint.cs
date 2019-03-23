using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootpoint : MonoBehaviour {
    Player player;


	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	

	void Update () {
        transform.LookAt(player.transform);
	}
}
