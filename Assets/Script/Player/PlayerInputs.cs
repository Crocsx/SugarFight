using UnityEngine;
using System.Collections;

public class PlayerInputs : MonoBehaviour {

    // PROPERTIES -----------------------------------------------------

    Transform           _transform;
    PlayerControl       _pController;
    PlayerHandler       _pHandler;
    Vector2             _currAxis;

    // INTERFACE -----------------------------------------------------

    void Start ()
    {
        _transform = transform;
        _pController = _transform.GetComponent<PlayerControl>();
        _pHandler = _transform.GetComponent<PlayerHandler>();
    }
	

	void Update ()
    {
        CheckInputs();
        _pController.onMove(new Vector3(_currAxis.y, 0, _currAxis.x));
    }

    // METHODS -----------------------------------------------------

    void CheckInputs()
    {
        _currAxis.x = Input.GetAxis(_pHandler.id + "_Vertical");
        _currAxis.y = Input.GetAxis(_pHandler.id + "_Horizontal");

        if(Input.GetButtonDown(_pHandler.id + "_Jump"))
            _pController.onJump();

        if (Input.GetButtonDown(_pHandler.id + "_Block"))
            _pController.Block(true);

        if (Input.GetButtonUp(_pHandler.id + "_Block"))
            _pController.Block(false);

        if (Input.GetButtonDown(_pHandler.id + "_Fire1"))
            _pController.onAttack(0);
        
        if (Input.GetButtonDown(_pHandler.id + "_Fire2"))
            _pController.onAttack(1);
    }
}
