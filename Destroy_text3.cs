using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_text3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ((Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal") > 0) && GameObject.Find ("Text3")) {
			Destroy (GameObject.Find ("Text3"));
		}
		
	}
}
