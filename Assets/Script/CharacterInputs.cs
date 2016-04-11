using UnityEngine;
using System.Collections;

public class CharacterInputs : MonoBehaviour {
    Transform _transform;
    PlayerController _Controller;
    Vector2 _currAxis;

    // Use this for initialization
    void Start () {
        _transform = transform;
        _Controller = _transform.GetComponent<PlayerController>();

    }
	
	// Update is called once per frame
	void Update () {
        checkInputs();
        _Controller.axisUpdate(new Vector3(_currAxis.y, 0, _currAxis.x));
    }

    void checkInputs(){
        _currAxis.x = Input.GetAxis("Vertical");
        _currAxis.y = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
            _Controller.onJump();

    }
}
