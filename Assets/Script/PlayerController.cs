using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // Properties

    // Jump Params
    public float jumpHeight = 1;
    
    // Move Params
    public float walkSpeed = 10;
    public float fallSpeed = 0.05f;

    // Default
    Vector3 movement = Vector3.zero;
    Transform _transform;
    CharacterController _cController;

    void Awake()
    {
        _transform = transform;
        _cController = _transform.GetComponent<CharacterController>();
    }

    void Start() { 
    }
    
    void FixedUpdate()
    {
        ApplyGravity();
        Move();
    }

    public void Move()
    {
        _cController.Move(movement);
    }

    public void axisUpdate(Vector3 axis)
    {
        movement.x = axis.x * walkSpeed * Time.deltaTime;
        movement.z = axis.z * walkSpeed * Time.deltaTime;
    }
    
    public void onJump()
    {
        if (_cController.isGrounded)
        {
            movement.y = 0;
            movement.y += jumpHeight;
        }
    }

    void ApplyGravity()
    {
        if (!_cController.isGrounded)
            movement.y -= fallSpeed;
    }
}
