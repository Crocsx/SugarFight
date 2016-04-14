using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    // EVENTS -----------------------------------------------------

    public delegate void OnGroundedEvent();
    public event OnGroundedEvent OnGrounded;

    // PROPERTIES ------------------------------------------------- 

    public delegate void onFixedUpdate();
    public onFixedUpdate OnFixedUpdate;

    [Header("JUMP PARAMS")]
    public float jumpForce = 12;
    public float jumpDuration = 0.5f;
    public float jumpSlow = 0.8f;
    float _jmpDuration = 0;
    float _jmpForce = 0;

    [Header("MOVE PARAMS")]
    public float moveSpeed = 1;

    Rigidbody _rigidbody;
    Animator _animator;
    Transform _transform;
    Vector3 _initRotation;

    [HideInInspector]
    public bool disableMovement = false;

    bool _isInvulnerable = false;
    bool isInvulnerable
    {
        get { return _isInvulnerable; }
        set { _isInvulnerable = value; }
    }

    bool _isJumping;
    bool isJumping
    {
        get { return _isJumping; }
        set { _isJumping = value; }
    }

    bool _isGrounded;
    bool isGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; _animator.SetBool("OnGround", value); }
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
        set { _isFalling = value; _animator.SetBool("isFalling", value); }
    }
    // INTERFACE ------------------------------------------------- 

    void Awake()
    {
        _transform = transform;
        _rigidbody = _transform.GetComponent<Rigidbody>();
        _animator = _transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        movement = Vector3.zero;

        if(isOnGround())
            OnLeaveGround();
        else
            OnLeaveGround();

        _initRotation = transform.eulerAngles;

        OnFixedUpdate += Fall;
        OnFixedUpdate += Move;
    }

    void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            OnEnterGround();
        }
        if (collision.collider.CompareTag("Player"))
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            OnLeaveGround();
        }
    }

    // STATES --------------------------------------------------

    void Jump()
    {
        _jmpDuration -= Time.fixedDeltaTime;
        _jmpForce += Time.fixedDeltaTime;
        if (_jmpDuration > 0)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jmpForce, _rigidbody.velocity.z);
        }
        else
        {
            OnFixedUpdate -= Jump;
            OnGrounded += ResetJump;
        }
    }

    void Fall()
    {
        if (_rigidbody.velocity.y < 0)
        {
            isFalling = true;
            OnFixedUpdate -= Fall;
        }
    }

    // METHODS --------------------------------------------------

    public void onMove(Vector3 axis)
    {
        movement = new Vector3(axis.x, 0, axis.z);
    }

    public void Move()
    {
        if (disableMovement)
            return;

        ScaleCheck(_movement);
        _rigidbody.AddForce(_movement * moveSpeed * Time.fixedDeltaTime * 1000);
    }

    public void onJump()
    {
        if(isGrounded)
            OnFixedUpdate += Jump;
    }

    void ResetJump()
    {
        _jmpForce = jumpForce;
        _jmpDuration = jumpDuration;
        OnGrounded -= ResetJump;
    }

    void OnEnterGround()
    {
        if (OnGrounded != null)
            OnGrounded();

        AugmentSpeed(jumpSlow);
        isFalling = false;
        isGrounded = true;
    }

    void OnLeaveGround()
    {
        ReduceSpeed(jumpSlow);
        isGrounded = false;
        OnFixedUpdate += Fall;
    }

    public void ScaleCheck(Vector3 axis)
    {
        if (axis.x < 0)
            _transform.localScale = new Vector3(1, 1, 1);
        else if (axis.x > 0)
            _transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Reset()
    {
        _rigidbody.velocity = Vector3.zero;
        ResetJump();
    }

    public void ReduceSpeed(float slow)
    {
        if (slow == 1)
            disableMovement = true;
        else
            moveSpeed = moveSpeed * (1 - slow);
    }

    public void AugmentSpeed(float boost)
    {
        if (boost == 1)
            disableMovement = false;
        else
            moveSpeed = moveSpeed / (1 - boost);
    }

    bool isOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }
}
