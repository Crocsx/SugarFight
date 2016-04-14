using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {

    // EVENTS -----------------------------------------------------

    public delegate void OnDeathEvent(string pName, Transform transform);
    public event OnDeathEvent OnDeath;

    // PROPERTIES -------------------------------------------------

    public Transform pMaker;
    [HideInInspector]
    public string id;

    PlayerControl _pControl;
    PlayerInputs _pInputs;
    PlayerFighter _pFighter;
    Transform _transform;

    Color _color;
    // INTERFACE -------------------------------------------------

    void Awake () {
        _transform = transform;
        _pControl = _transform.GetComponent<PlayerControl>();
        _pInputs = _transform.GetComponent<PlayerInputs>();
        _pFighter = _transform.GetComponent<PlayerFighter>();
    }
	
	void Update () {
        RaycastHit hit;
	    if(Physics.Raycast(transform.position, Vector3.down, out hit)){ 
            pMaker.transform.position = hit.point;
        }
	}

    public void Setup(string nid, Color nColor)
    {
        id = nid;
        _color = nColor;
        pMaker.GetComponent<SpriteRenderer>().color = nColor;
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
