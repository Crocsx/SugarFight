using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ControllerMapping : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*Debug.Log (Input.inputString);*/
		Debug.Log ("Penis");

		foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kcode))
				Debug.Log("KeyCode down: " + kcode);
		}
	}
}
