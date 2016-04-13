using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject bgClickToPlay;

    bool isClicked;

    private static MenuManager _instance;
    private MenuManager() { }
    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("MenuManager");
                go.AddComponent<MenuManager>();
                _instance = go.GetComponent<MenuManager>();
            }
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void salut()
    {
        Debug.Log("salut");
    }

    public void TitlecardClickedFirstTime()
    {
        Debug.Log("function");
        if(!isClicked)
        {
            Debug.Log("clicked");
            isClicked = true;
            bgClickToPlay.transform.Translate(10, 0, 0);
        }
    }
}
