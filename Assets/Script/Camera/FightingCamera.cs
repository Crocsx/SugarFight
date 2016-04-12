using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCamera : MonoBehaviour {

    // Expecting 2 players only
    public GameObject _plane;


    //GameObject clone;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(_plane.transform);
    }


    void CreateClone(Vector3 myPosition)
    {
    }
}
