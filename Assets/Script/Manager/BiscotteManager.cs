﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiscotteManager : MonoBehaviour {

    public List<GameObject> biscotteMorceau = new List<GameObject>();
    public GameObject biscotteEntiere;

    public float timeBreakpoint = 19f;//secondes


    float _timer = 0;
    bool isActive = false;
    bool isBreak = false;

    void Awake()
    {
        isActive = false;
        isBreak = false;
        EventManager.StartListening("OnStageStart", StartCount);
    }
    // Use this for initialization
    void Start () {
        biscotteEntiere.SetActive(true);

        foreach (GameObject biscotte in biscotteMorceau)
        {
            biscotte.SetActive(false);
        }
    }
	
    void StartCount()
    {
        isActive = true;
    }
    // Update is called once per frame
    void Update () {
        if (!isActive || isBreak)
            return;

        _timer += Time.deltaTime;

        if (_timer >= timeBreakpoint)
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
