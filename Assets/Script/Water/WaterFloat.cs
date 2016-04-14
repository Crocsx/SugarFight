using UnityEngine;
using System.Collections;

public class WaterFloat : MonoBehaviour {

    public float waterLevel = 0f;
    public float floatHeight = 0.1f;
    public float bounceDamp = 0.04f;
    public Vector3 buoyancyCenterOffset;
    public float forceFactor;
    public Vector3 actionPoint;
    public Vector3 upLift;
    public float coeffTorque = 1;

    private Rigidbody _rigidbody;

    Quaternion startQuat;

    // init, that work even if script is disabled.
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startQuat = _rigidbody.rotation;
    }

	/*
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        startQuat = _rigidbody.rotation;
	}*/

	
	// Update is called once per frame
	void FixedUpdate () {
        actionPoint = transform.position + transform.TransformDirection(buoyancyCenterOffset);
        forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * bounceDamp);
            _rigidbody.AddForceAtPosition(upLift, actionPoint);
        }

        //
        Quaternion currQuat = _rigidbody.rotation;
        Quaternion rotQuat = Quaternion.FromToRotation(transform.up, Vector3.up);

        float angle;
        Vector3 vector;
        rotQuat.ToAngleAxis(out angle, out vector);

        _rigidbody.AddTorque(vector * angle * coeffTorque);
	}




}
