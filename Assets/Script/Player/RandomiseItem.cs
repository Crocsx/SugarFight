using UnityEngine;
using System.Collections;

public class RandomiseItem : MonoBehaviour {

    public GameObject[] Hat;
    public GameObject[] Glass;
    public GameObject[] Back;

    PlayerHandler _pHandler;

    // Use this for initialization
    void Start () {
        _pHandler = transform.GetComponent<PlayerHandler>();
        _pHandler.OnDeath += Randomise;
    }
	
	// Update is called once per frame
	void Randomise (string pName, Transform transform) {

	    for (var i = 0; i< Hat.Length; i++)
        {
            Hat[i].SetActive(false);
        }
        for (var i = 0; i < Glass.Length; i++)
        {
            Glass[i].SetActive(false);
        }
        for (var i = 0; i < Back.Length; i++)
        {
            Back[i].SetActive(false);
        }

        if(Random.Range(0.0f, 1.0f) > 0.2)
            Hat[Random.Range(0, Hat.Length - 1)].SetActive(true);
        if (Random.Range(0.0f, 0.5f) > 0.5)
            Glass[Random.Range(0, Glass.Length - 1)].SetActive(true);
        if (Random.Range(0.0f, 0.5f) > 0.5)
            Back[Random.Range(0, Back.Length - 1)].SetActive(true);
    }
}
