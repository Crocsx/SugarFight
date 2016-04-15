using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIinGame : MonoBehaviour {

    public Slider slider;
    public GameObject sugar1;
    public GameObject sugar2;
    public GameObject sugar3;
    public GameObject sugar4;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (slider.value == 0)
        {
            sugar1.SetActive(false);
            sugar2.SetActive(false);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (slider.value == 1)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(false);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (slider.value == 2)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(false);
            sugar4.SetActive(false);
        }
        else if (slider.value == 3)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(true);
            sugar4.SetActive(false);
        }
        else if (slider.value == 4)
        {
            sugar1.SetActive(true);
            sugar2.SetActive(true);
            sugar3.SetActive(true);
            sugar4.SetActive(true);
        }
    }

}
