using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiscotteManager : MonoBehaviour {

    public List<GameObject> biscotteMorceau = new List<GameObject>();
    public GameObject biscotteEntiere;

    public float timeBreakpoint = 19f;//secondes

    bool isActive = false;
    bool isBreak = false;

    void Awake()
    {
        isActive = false;
        EventManager.StartListening("OnStageStart", StartCount);
    }
    // Use this for initialization
    void Start () {
    }
	
    void StartCount()
    {

        isActive = true;
    }
    // Update is called once per frame
    void Update () {
        if (!isActive)
            return;

	    if (Time.time >= timeBreakpoint && !isBreak)
        {
            biscotteEntiere.SetActive(false);

            foreach (GameObject biscotte in biscotteMorceau)
            {
                biscotte.SetActive(true);
            }

            isBreak = true;
        }
	}
}
