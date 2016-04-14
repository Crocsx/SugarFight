using UnityEngine;
using System.Collections;
using Rewired;

public class PlayerInputs : MonoBehaviour {

    // PROPERTIES -----------------------------------------------------

    Transform           _transform;
    PlayerControl       _pController;
    PlayerFighter        _pFighter;
    PlayerHandler       _pHandler;

    [HideInInspector]
    public Vector2      _currAxis;

    Player              _player;

    // INTERFACE -----------------------------------------------------

    void Start ()
    {
        _transform = transform;
        _pController = _transform.GetComponent<PlayerControl>();
        _pFighter = _transform.GetComponent<PlayerFighter>();
        _pHandler = _transform.GetComponent<PlayerHandler>();
        _player = ReInput.players.GetPlayer(_pHandler.id);
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
        _currAxis.x = _player.GetAxis("Vertical");
        _currAxis.y = _player.GetAxis("Horizontal");
    }

    void CheckInputs()
    {

        if(_player.GetButtonDown("Jump"))
            _pController.onJump();

        if (_player.GetButtonDown("Fire1"))
            _pFighter.onAttack(0);
        if (_player.GetButtonUp("Fire1"))
            _pFighter.askStopAttack(0);

        if (_player.GetButtonDown("Fire2"))
            _pFighter.onAttack(1);
        if (_player.GetButtonUp("Fire2"))
            _pFighter.askStopAttack(1);

        if (_player.GetButtonDown("Fire3"))
            _pFighter.onAttack(3); 
        if (_player.GetButtonUp("Fire3"))
            _pFighter.askStopAttack(3);

        if (_player.GetButtonDown("Block"))
            _pFighter.Block(true);
        if (_player.GetButtonUp("Block"))
            _pFighter.Block(false);

        if (_player.GetButtonDown("Dash"))
            _pFighter.onAttack(4);
        if (_player.GetButtonUp("Dash"))
            _pFighter.askStopAttack(4);
        
        if (Input.GetButtonDown("Start"))
            EventManager.TriggerEvent("OnPause");
        if (Input.GetButtonUp("Start"))
            EventManager.TriggerEvent("UnPause");
    }
}
