using UnityEngine;
using System.Collections;

public class PlayerFighter : MonoBehaviour {

    public delegate void onUpdate();
    public onUpdate OnUpdate;

    [Header("FIGHT PARAMS")]

    public float TimeInvulnerable = 1.0f;
    float noDamageTimer = 0;
    public Attack[] attacks = new Attack[2];

    [Header("BLOCK PARAMS")]
    [Range(0, 1)]
    public float blockSlowAmount = 0.2f;

    Rigidbody _rigidbody;
    Animator _animator;
    Transform _transform;
    PlayerControl _pControl;

    
    bool _isDamaged;
    bool isDamaged
    {
        get { return _isDamaged; }
        set { _isDamaged = value; if (_isDamaged) { _animator.SetTrigger("Damage"); } }
    }

    int _currentAttack = -1;
    int currentAttack
    {
        get { return _currentAttack; }
        set
        {
            if (value != -1)
                _animator.SetBool(attacks[value].animName, true);
            else
                _animator.SetBool(attacks[_currentAttack].animName, false);
            _currentAttack = value;
        }
    }

    bool _isBlocking = false;
    bool isBlocking
    {
        get { return _isBlocking; }
        set { _isBlocking = value; _animator.SetBool("Blocking", value); }
    }

    void Start () {
        _transform = transform;
        _pControl = _transform.GetComponent<PlayerControl>();
        _rigidbody = _transform.GetComponent<Rigidbody>();
        _animator = _transform.GetChild(0).GetComponent<Animator>();

        for (var i = 0; i < attacks.Length; i++)
        {
            attacks[i].Setup(_transform);
        }
    }

    // Update is called once per frame
    void Update () {
        if (OnUpdate != null)
            OnUpdate();
    }

    public void onAttack(int value)
    {
        if (currentAttack != -1 && attacks[currentAttack].isAttackPlaying)
            return;

        _pControl.disableMovement = true;
        currentAttack = value;
        attacks[currentAttack].Start();
        OnUpdate += attacks[currentAttack].OnUpdate;
        attacks[currentAttack].OnAttackEnd += StopAttack;
    }

    public void askStopAttack(int value)
    {
        if (attacks[value].isAttackPlaying)
        {
            attacks[value].Interrupt();
        }
    }

    void StopAttack()
    {
        _pControl.disableMovement = false;
        OnUpdate -= attacks[currentAttack].OnUpdate;
        attacks[currentAttack].OnAttackEnd -= StopAttack;
        currentAttack = -1;
    }

    public void OnDamaged(float damage, Transform player)
    {
        if (!isDamaged && !isBlocking)
        {
            isDamaged = true;
            Vector3 dir = player.transform.position - _transform.position;
            _rigidbody.AddForce(-dir * damage * 1000);
            OnUpdate += Damage;
        }
    }

    public void Block(bool value)
    {
        _rigidbody.velocity = Vector3.zero;

        if (value)
            _pControl.ReduceSpeed(blockSlowAmount);
        else
            _pControl.AugmentSpeed(blockSlowAmount);

        isBlocking = value;
    }

    void Damage()
    {
        noDamageTimer += Time.deltaTime;
        if (noDamageTimer > TimeInvulnerable)
        {
            isDamaged = false;
            noDamageTimer = 0;
            OnUpdate -= Damage;
        }
    }
}
