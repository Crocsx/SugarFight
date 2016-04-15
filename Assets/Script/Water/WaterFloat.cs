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
    public GameObject pivotCenter = null;

    private Rigidbody _rigidbody;

    Quaternion startQuat;

    // init, that work even if script is disabled.
    void Awake()
    {
        if (pivotCenter != null) ConfigActionPoint(pivotCenter);
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

    // permet de positionner buoyancyCenterOffset (offset) à partir d'un vector (tiré d'un gameObject de la scène)
    // ainsi que le offset y poru center au milieu de l'eau (pas sûr de cela)
    // désactive le gameObject passé en param
    void ConfigActionPoint(GameObject pivotCenter)
    {
        Vector3 vec3Center = pivotCenter.transform.position;

        buoyancyCenterOffset.x = vec3Center.x - transform.position.x;
        buoyancyCenterOffset.z = vec3Center.z - transform.position.z;
        buoyancyCenterOffset.y = transform.localScale.y / 2; // scale == width in flash ?

        pivotCenter.SetActive(false);
    }


}
