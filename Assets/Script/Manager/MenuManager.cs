using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Camera cameraMenu;

    public GameObject titlecardPopin;
    public GameObject txtOptions;
    public GameObject txtRules;
    public GameObject txtCredits;

    public GameObject btnFirstClic;
    public GameObject background;
    public GameObject background2;
    public GameObject title;
    public GameObject bgClickToPlay;
    public GameObject btnPlay;
    public GameObject btnRules;
    public GameObject btnOptions;
    public GameObject btnCredits;
    public GameObject titlecard_bgAlpha;

    bool isClicked;
    bool isPopin;

    string myCurrentPopin;

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
        txtOptions.SetActive(false);
        txtRules.SetActive(false);
        txtCredits.SetActive(false);
        btnFirstClic.SetActive(true);
        background.SetActive(true);
        background2.SetActive(true);
        title.SetActive(true);
        btnPlay.SetActive(false);
        btnRules.SetActive(false);
        btnOptions.SetActive(false);
        btnCredits.SetActive(false);
        titlecard_bgAlpha.SetActive(false);
        titlecardPopin.SetActive(false);
    }

    /* MENU : ouverture de la scène de jeu */
	public void OnClickBtnPlay()
    {
        SceneManager.LoadScene("MAIN_STAGE");
    }

    /* MENU : clic sur un bouton OPTIONS, CREDITS, RULES */
    public void OnClickButton(string myBtn)
    {
        if (!isPopin)
        {
            myCurrentPopin = myBtn;
            isPopin = true;
            titlecardPopin.SetActive(true);
            titlecard_bgAlpha.SetActive(true);
            if (myBtn == "options")
            {
                txtOptions.SetActive(true);
            }
            else if (myBtn == "rules")
            {
                txtRules.SetActive(true);
            }
            else if (myBtn == "credits")
            {
                txtCredits.SetActive(true);
            }
            
        }
        
    }

    /* POPIN : retour au menu */
    public void OnClickOutPopin()
    {
        if (isPopin)
        {
            isPopin = false;
            titlecardPopin.SetActive(false);
            titlecard_bgAlpha.SetActive(false);
            if (myCurrentPopin == "options")
            {
                txtOptions.SetActive(false);
            }
            else if (myCurrentPopin == "rules")
            {
                txtRules.SetActive(false);
            }
            else if (myCurrentPopin == "credits")
            {
                txtCredits.SetActive(false);
            }
            
        }
    }

    /* TITLECARD : ouverture du menu */
    public void TitlecardClickedFirstTime()
    {
        if(!isClicked)
        {
            isClicked = true;
            btnFirstClic.SetActive(false);
            background2.SetActive(false);
            StartCoroutine(BgClickToPlayMove());
        }
    }

    IEnumerator MoveCameraMenu()
    {
        Vector3 startPosition = cameraMenu.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, -10, 50);
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            cameraMenu.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator BgClickToPlayMove()
    {
        bgClickToPlay.SetActive(false);
        btnPlay.SetActive(true);
        btnRules.SetActive(true);
        btnOptions.SetActive(true);
        btnCredits.SetActive(true);

        Vector3 startPositionPlayBtn = btnPlay.transform.position;
        Vector3 endPositionPlayBtn = startPositionPlayBtn + new Vector3(110, 0, 0);

        Vector3 startPositionRulesBtn = btnRules.transform.position;
        Vector3 endPositionRulesBtn = startPositionRulesBtn + new Vector3(-110, 0, 0);

        Vector3 startPositionOptionBtn = btnOptions.transform.position;
        Vector3 endPositionOptionBtn = startPositionOptionBtn + new Vector3(90, 0, 0);

        Vector3 startPositionCreditsBtn = btnCredits.transform.position;
        Vector3 endPositionCreditsBtn = startPositionCreditsBtn + new Vector3(-90, 0, 0);

        float elapsedTime = 0;
        float duration = 0.5f;
        
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
