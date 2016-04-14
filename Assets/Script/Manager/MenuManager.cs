using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject bgClickToPlay;
    public GameObject btnPlay;
    public GameObject btnRules;
    public GameObject btnOptions;
    public GameObject btnCredits;

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

    void Start () {
        btnPlay.SetActive(false);
        btnRules.SetActive(false);
        btnOptions.SetActive(false);
        btnCredits.SetActive(false);
	}
	

    public void TitlecardClickedFirstTime()
    {
        if(!isClicked)
        {
            isClicked = true;
            StartCoroutine(BgClickToPlayMove());
        }
    }

    // L'élément "Click to Play" du Titlecar se retire progressivement
    IEnumerator BgClickToPlayMove()
    {
        Vector3 startPosition = bgClickToPlay.transform.position;
        Vector3 endPosition = startPosition + new Vector3(250, 0, 0);
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            bgClickToPlay.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bgClickToPlay.SetActive(false);
        btnPlay.SetActive(true);
        btnRules.SetActive(true);
        btnOptions.SetActive(true);
        btnCredits.SetActive(true);

        Vector3 startPositionPlayBtn = btnPlay.transform.position;
        Vector3 endPositionPlayBtn = startPositionPlayBtn + new Vector3(250, 0, 0);

        Vector3 startPositionRulesBtn = btnRules.transform.position;
        Vector3 endPositionRulesBtn = startPositionRulesBtn + new Vector3(-250, 0, 0);

        Vector3 startPositionOptionBtn = btnOptions.transform.position;
        Vector3 endPositionOptionBtn = startPositionOptionBtn + new Vector3(250, 0, 0);

        Vector3 startPositionCreditsBtn = btnCredits.transform.position;
        Vector3 endPositionCreditsBtn = startPositionCreditsBtn + new Vector3(-250, 0, 0);

        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            btnPlay.transform.position = Vector3.Lerp(startPositionPlayBtn, endPositionPlayBtn, elapsedTime / duration);
            btnRules.transform.position = Vector3.Lerp(startPositionRulesBtn, endPositionRulesBtn, elapsedTime / duration);
            btnOptions.transform.position = Vector3.Lerp(startPositionOptionBtn, endPositionOptionBtn, elapsedTime / duration);
            btnCredits.transform.position = Vector3.Lerp(startPositionCreditsBtn, endPositionCreditsBtn, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}
