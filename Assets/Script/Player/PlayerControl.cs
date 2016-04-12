using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    // EVENTS -----------------------------------------------------

    public delegate void OnDeathEvent(string pName, Transform transform);
    public event OnDeathEvent OnDeath;

    // PROPERTIES ------------------------------------------------- 

    public delegate void onUpdate();
    public onUpdate OnUpdate;

    // JUMP PARAMS
    public float jumpForce = 12;
    public float jumpDuration = 0.5f;
    float _jmpDuration = 0;
    float _jmpForce = 0;

    // MOVE PARAMS
    public float moveSpeed = 1;

    // FIGHT PARAMS
    public float attackRate = 0.3f;
    bool[] attack = new bool[2];
    float[] attackTimer = new float[2];
    int[] timePressed = new int[2];

    // Generic
    [HideInInspector]
    public string id;

    Rigidbody _rigidbody;
    Animator _animator;
    Transform _transform;

    bool _isGrounded;
    bool isGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; _animator.SetBool("OnGround", value); }
    }

    bool _isJumping;
    bool isJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
    }

    Vector3 _movement;
    Vector3 movement
    {
        get { return _movement; }
        set { _movement = value; _animator.SetFloat("Movement", value.magnitude); }
    }

    bool _isFalling;
    bool isFalling
    {
        get { return _isFalling; }
        set { _isFalling = value; _animator.SetBool("isFalling", false); }
    }

    // INTERFACE ------------------------------------------------- 

    void Awake()
    {
        _transform  = transform;
        _rigidbody  = _transform.GetComponent<Rigidbody>();
        _animator   = _transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        _movement = Vector3.zero;
        OnUpdate += Move;
    }

    void FixedUpdate()
    {
        if (OnUpdate != null)
            OnUpdate();
    }

    // STATES --------------------------------------------------

    void Jump()
    {
        _jmpDuration -= Time.deltaTime;
        _jmpForce += Time.deltaTime;
        if (_jmpDuration > 0)
        {
            //_rigidbody.AddForce(Vector3.up * _jmpForce * Time.deltaTime * 1000);
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.y, _jmpForce);
        }
        else
        {
            OnUpdate -= Jump;
        }
    }

    // METHODS ------------------------------------------------- 

    public void Move()
    {
        _rigidbody.AddForce(_movement * moveSpeed * Time.deltaTime * 1000);
    }

    public void axisUpdate(Vector3 axis)
    {
        _movement = new Vector3(axis.x, 0, axis.z);
    }

    public void onJump()
    {
        OnUpdate += Jump;
    }

    public void askDie()
    {
        if (OnDeath != null)
            OnDeath(id, transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            OnEnterGround();
        }
    }
   

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            OnLeaveGround();
        }
    }

    void OnEnterGround()
    {
        isGrounded = true;
        isFalling = false;

        _jmpForce = jumpForce;
        _jmpDuration = jumpDuration;

    }

    void OnLeaveGround()
    {
        isGrounded = false;
    }
}
