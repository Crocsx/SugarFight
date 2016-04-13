using UnityEngine;
using System.Collections;

public class PlayerInputs : MonoBehaviour {

    // PROPERTIES -----------------------------------------------------

    Transform           _transform;
    PlayerControl       _pController;
    PlayerFigher        _pFighter;
    PlayerHandler       _pHandler;
    Vector2             _currAxis;

    bool _blockButton;
    bool blockButton
    {
        get { return blockButton; }
        set { blockButton = value; _pFighter.Block(value);}
    }

    // INTERFACE -----------------------------------------------------

    void Start ()
    {
        _transform = transform;
        _pController = _transform.GetComponent<PlayerControl>();
        _pFighter = _transform.GetComponent<PlayerFigher>();
        _pHandler = _transform.GetComponent<PlayerHandler>();
    }
	

	void Update ()
    {
        CheckAxis();
        CheckInputs();
        _pController.onMove(new Vector3(_currAxis.y, 0, _currAxis.x));
    }

    // METHODS -----------------------------------------------------

    void CheckAxis()
    {
        _currAxis.x = Input.GetAxis(_pHandler.id + "_Vertical");
        _currAxis.y = Input.GetAxis(_pHandler.id + "_Horizontal");

        if ((Input.GetAxis(_pHandler.id + "_Block") > 0) && !_blockButton)
            blockButton = true;
        if ((Input.GetAxis(_pHandler.id + "_Block") <= 0) && _blockButton)
            blockButton = false;
    }

    void CheckInputs()
    {
        _currAxis.x = Input.GetAxis(_pHandler.id + "_Vertical");
        _currAxis.y = Input.GetAxis(_pHandler.id + "_Horizontal");

        if(Input.GetButtonDown(_pHandler.id + "_Jump"))
            _pController.onJump();

        if (Input.GetButtonDown(_pHandler.id + "_Fire1"))
            _pFighter.onAttack(0);
        
        if (Input.GetButtonDown(_pHandler.id + "_Fire2"))
            _pFighter.onAttack(1);
    }
}
