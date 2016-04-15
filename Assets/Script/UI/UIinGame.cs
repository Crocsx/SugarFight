using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIinGame : MonoBehaviour {

    public playerReference entity;
    PlayerFighter _pFighter;
    public float percent;
    public GameObject sugar1;
    public GameObject sugar2;
    public GameObject sugar3;
    public GameObject sugar4;
    public Text _counter;
    bool isStarted = false;


    void Awake()
    {
        isStarted = false;
        EventManager.StartListening("PlayerDeath", OnLoseHP);
    }

    public void EnableScript(playerReference _fighter)
    {
        enabled = true;
        entity = _fighter;
        _pFighter  = _fighter.reference.transform.GetComponent<PlayerFighter>();
        isStarted = true;
    }
    // Use this for initialization
    void Update() {
        if (!isStarted)
            return;

        percent = _pFighter.multiplicator;
        _counter.text = (percent*10) + " %";
    }
	
	// Update is called once per frame
	void OnLoseHP() {
        int value = entity.lifeRemaining;
        if (value == 0)
        {
            sugar1.SetActive(false);
            sugar2.SetActive(false);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (value == 1)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(false);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (value == 2)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (value == 3)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(true);
            sugar4.SetActive(false);
        }
        else if (value == 4)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(true);
            sugar4.SetActive(true);
        }
    }

}
