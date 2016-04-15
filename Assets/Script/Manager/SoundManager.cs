using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    private static SoundManager _instance;
    private SoundManager() { }
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SoundManager");
                go.AddComponent<SoundManager>();
                _instance = go.GetComponent<SoundManager>();
            }
            return _instance;
        }
    }

    void Start()
    {

    }

}
