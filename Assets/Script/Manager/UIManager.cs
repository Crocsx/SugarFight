using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public UIinGame _p1;
    public UIinGame _p2;
    public StageManager _stageManager;
    // Use this for initialization
    void Awake () {
        EventManager.StartListening("OnStageStart", Setup);
    }


    void Setup()
    {
        _p1.EnableScript(_stageManager.lifeRemaining["Player0"]);
        _p2.EnableScript(_stageManager.lifeRemaining["Player1"]);
    }
   
}
