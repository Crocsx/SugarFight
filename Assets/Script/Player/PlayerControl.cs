using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    // EVENTS -----------------------------------------------------

    public delegate void OnGroundedEvent();
    public event OnGroundedEvent OnGrounded;

    // PROPERTIES ------------------------------------------------- 

    public delegate void onUpdate();
    public onUpdate OnUpdate;

    [Header("JUMP PARAMS")]
    public float jumpForce = 12;
    public float jumpDuration = 0.5f;
    float _jmpDuration = 0;
    float _jmpForce = 0;

    [Header("MOVE PARAMS")]
    public float moveSpeed = 1;

    [Header("FIGHT PARAMS")]
    public float attackRate = 0.3f;
    public float TimeInvulnerable = 1.0f;
    float noDamageTimer = 0;
    public Attack[] attacks = new Attack[2];

    [Header("BLOCK PARAMS")]
    [Range(0, 1)]
    public float blockSlowAmount = 0.2f;

    Rigidbody _rigidbody;
    Animator _animator;
    Transform _transform;
    Vector3 _initRotation;

    bool _isDamaged;
    bool isDamaged
    {
        get { return _isDamaged; }
        set { _isDamaged = value; if (_isDamaged) { _animator.SetTrigger("Damage"); } }
    }

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

    bool _isBlocking = false;
    bool isBlocking
    {
        get { return _isBlocking; }
        set { _isBlocking = value; _animator.SetBool("Blocking", value); }
    }

    int _currentAttack = -1;
    int currentAttack
    {
        get { return _currentAttack; }
        set {
            if (value != -1)
                _animator.SetBool(attacks[value].animName, true);
            else
                _animator.SetBool(attacks[_currentAttack].animName, false);
            _currentAttack = value;
        }
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
        Reset();
        _initRotation = transform.eulerAngles;

        for (var i = 0; i < attacks.Length; i++)
        {
            attacks[i].script.Setup(attacks[i], _transform);
        }

        OnUpdate += Fall;
        OnUpdate += Move;
    }

    void FixedUpdate()
    {
        if (OnUpdate != null)
            OnUpdate();
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
            OnUpdate -= Jump;
            OnGrounded += ResetJump;
        }
    }

    void Fall()
    {
        if (_rigidbody.velocity.y < 0)
        {
            isFalling = true;
            OnUpdate -= Fall;
        }
    }

    void Attack() {
        attacks[currentAttack].attack = true;
        attacks[currentAttack].attackTimer = 0;
        attacks[currentAttack].timePressed++;
        if (attacks[currentAttack].attack)
        {
            attacks[currentAttack].attackTimer += Time.fixedDeltaTime;

            if (attacks[currentAttack].attackTimer > attackRate || attacks[currentAttack].timePressed >= 4)
            {
                StopAttack();
            }
        }
    }

    void Damage()
    {
        noDamageTimer += Time.fixedDeltaTime;
        if (noDamageTimer > TimeInvulnerable)
        {
            isDamaged = false;
            noDamageTimer = 0;
            OnUpdate -= Damage;
        }   
    }

    // METHODS --------------------------------------------------

    public void Block(bool value)
    {
        _rigidbody.velocity = Vector3.zero;

        if (value)
            ReduceSpeed(blockSlowAmount);
        else
            AugmentSpeed(blockSlowAmount);

        isBlocking = value;
    }


    public void onMove(Vector3 axis)
    {
        movement = new Vector3(axis.x, 0, axis.z);
    }

    public void Move()
    {
        ScaleCheck(_movement);
        _rigidbody.AddForce(_movement * moveSpeed * Time.fixedDeltaTime * 1000);
    }

    public void onJump()
    {
        OnUpdate += Jump;
    }

    void ResetJump()
    {
        _jmpForce = jumpForce;
        _jmpDuration = jumpDuration;
        OnGrounded -= ResetJump;
    }

    public void onAttack(int value)
    {
        currentAttack = value;
        OnUpdate += Attack;
    }

    void StopAttack()
    {
        OnUpdate -= Attack;

        attacks[currentAttack].attack = false;
        attacks[currentAttack].attackTimer = 0;
        attacks[currentAttack].timePressed = 0;
        currentAttack = -1;
    }

    public void OnDamaged(float damage, Transform player)
    {
        if (!isDamaged && !isBlocking)
        {
            isDamaged = true;
            Vector3 dir = player.transform.position - _transform.position;
            _rigidbody.AddForce(-dir * damage * 150);
            OnUpdate += Damage;
        }
    }

    void OnEnterGround()
    {
        if (OnGrounded != null)
            OnGrounded();

        isFalling = false;
        isGrounded = true;
    }

    void OnLeaveGround()
    {
        isGrounded = false;
        OnUpdate += Fall;
    }

    void ScaleCheck(Vector3 axis)
    {
        if (axis.x < 0)
            _transform.localScale = new Vector3(1, 1, 1);
        else if (axis.x > 0)
            _transform.localScale = new Vector3(-1, 1, 1);

        if (axis.y < 0) 
            transform.rotation = Quaternion.Euler(_initRotation.x, _initRotation.y - 45, _initRotation.z);
        else if (axis.y > 0)
            transform.rotation = Quaternion.Euler(_initRotation.x, _initRotation.y + 45, _initRotation.z);
        else if(axis.y == 0.0f)
            transform.rotation = Quaternion.Euler(_initRotation.x, _initRotation.y, _initRotation.z);
    }

    public void Reset()
    {
        _rigidbody.velocity = Vector3.zero;
        ResetJump();
    }

    void ReduceSpeed(float slow)
    {
        moveSpeed = moveSpeed * (1 - slow);
    }

    void AugmentSpeed(float boost)
    {
        moveSpeed = moveSpeed / (1 - boost);
    }
}
