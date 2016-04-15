using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public UIinGame _p1;
    public UIinGame _p2;
    public StageManager _stageManager;
    public GameObject WinText;

    // Use this for initialization
    void Awake () {
        EventManager.StartListening("OnStageStart", Setup);
        EventManager.StartListening("EndGame", onEnd);
    }

    void onEnd()
    {
        WinText.GetComponent<Text>().text = "Player " +(_stageManager.loser+1) + " Lose !!!";
        WinText.SetActive(true);
    }
    void Setup()
    {
        _p1.EnableScript(_stageManager.lifeRemaining["Player0"]);
        _p2.EnableScript(_stageManager.lifeRemaining["Player1"]);
    }
   
}
