using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_collision : MonoBehaviour {

	int treasure_collected = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Treasure")) {
			col.gameObject.SetActive (false);
			treasure_collected ++;
		}
	}
}
