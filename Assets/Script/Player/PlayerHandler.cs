using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {

    // EVENTS -----------------------------------------------------

    public delegate void OnDeathEvent(string pName, Transform transform);
    public event OnDeathEvent OnDeath;

    // PROPERTIES -------------------------------------------------
    PlayerControl _pControl;
    PlayerInputs _pInputs;
    PlayerFigher _pFighter;
    Transform _transform;

    [HideInInspector]
    public string id;

    // INTERFACE -------------------------------------------------

    void Awake () {
        _transform = transform;
        _pControl = _transform.GetComponent<PlayerControl>();
        _pInputs = _transform.GetComponent<PlayerInputs>();
        _pFighter = _transform.GetComponent<PlayerFigher>();
    }
	
	void Update () {
	
	}


    // METHODS -------------------------------------------------

    public void askDie()
    {
        if (OnDeath != null)
            OnDeath(id, transform);
    }

    public void Disable()
    {
        _transform.GetComponent<Rigidbody>().isKinematic = true;
        _pControl.Reset();
        _pControl.enabled = false;
        _pInputs.enabled = false;
    }

    public void Enable()
    {
        _transform.GetComponent<Rigidbody>().isKinematic = false;
        _pControl.Reset();
        _pControl.enabled = true;
        _pInputs.enabled = true;
    }
}
