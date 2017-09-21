using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_text : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("Interact") && GameObject.Find ("Text2")) {
			Destroy (GameObject.Find ("Text2"));
		}
	}
}
