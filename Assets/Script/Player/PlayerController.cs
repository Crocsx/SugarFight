using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // EVENTS -----------------------------------------------------

    public delegate void OnDeathEvent(string pName, Transform transform);
    public event OnDeathEvent OnDeath;

    // PROPERTIES ------------------------------------------------- 

    public float jumpHeight = 1;
    public float walkSpeed  = 10;
    public float fallSpeed  = 0.05f;

    [HideInInspector]
    public string id;

    Vector3             _movement;
    CharacterController _cController;
    Transform           _transform;

    // INTERFACE ------------------------------------------------- 

    void Awake()
    {
        _transform = transform;
        _cController = _transform.GetComponent<CharacterController>();
    }

    void Start() {
        _movement = Vector3.zero;

    }
    
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    // METHODS ------------------------------------------------- 

    public void Move()
    {
        _cController.Move(_movement);
    }

    public void axisUpdate(Vector3 axis)
    {
        _movement.x = axis.x * walkSpeed * Time.deltaTime;
        _movement.z = axis.z * walkSpeed * Time.deltaTime;
    }
    
    public void onJump()
    {
        if (_cController.isGrounded)
        {
            _movement.y = 0;
            _movement.y += jumpHeight;
        }
    }

    void ApplyGravity()
    {
        if (!_cController.isGrounded)
            _movement.y -= fallSpeed;
    }

    public void askDie()
    {
        if(OnDeath != null)
            OnDeath(id, transform);
    }
}
