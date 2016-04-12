using UnityEngine;
using System.Collections;

public class PlayerInputs : MonoBehaviour {

    // PROPERTIES -----------------------------------------------------

    Transform           _transform;
    PlayerControl      _Controller;
    Vector2             _currAxis;

    // INTERFACE -----------------------------------------------------

    void Start ()
    {
        _transform = transform;
        _Controller = _transform.GetComponent<PlayerControl>();
    }
	

	void Update ()
    {
        CheckInputs();
        _Controller.axisUpdate(new Vector3(_currAxis.y, 0, _currAxis.x));
    }

    // METHODS -----------------------------------------------------

    void CheckInputs()
    {
        _currAxis.x = Input.GetAxis("Vertical");
        _currAxis.y = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
            _Controller.onJump();
    }
}
