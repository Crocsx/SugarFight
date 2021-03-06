﻿using UnityEngine;
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

    [HideInInspector]
    public float multiplicator;

    Rigidbody _rigidbody;
    Animator _animator;
    Transform _transform;
    PlayerControl _pControl;
    PlayerHandler _pHandler;
    ParticleSystem _pSystem;

    int maxParticles;

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

        for (var i = 0; i < attacks.Length; i++)
        {
            attacks[i].Setup(_transform);
        }

        _pControl = _transform.GetComponent<PlayerControl>();
        _rigidbody = _transform.GetComponent<Rigidbody>();
        _animator = _transform.GetChild(0).GetComponent<Animator>();
        _pHandler = _transform.GetComponent<PlayerHandler>();
        _pSystem = _transform.GetComponent<ParticleSystem>();
        maxParticles = _pSystem.maxParticles;
        _pHandler.OnDeath += Reset;
    }

    void Reset(string pName, Transform transform)
    {
        multiplicator = 0;
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
            _pSystem.Play();
            isDamaged = true;
            _pSystem.maxParticles += (int)(maxParticles + (multiplicator * 10));
            multiplicator += damage;
            Vector3 dir = player.transform.position - _transform.position;
            _pControl.ScaleCheck(dir);
            _rigidbody.AddForce(-dir * damage * 1000 * multiplicator);
            OnUpdate += Damage;
        }
    }

    public void Block(bool value)
    {
        _rigidbody.velocity = Vector3.zero;

        if (value && _pControl.isGrounded)
        {
            _pControl.ReduceSpeed(blockSlowAmount);
            isBlocking = value;
        }
        if (!value && isBlocking)
        {
            _pControl.AugmentSpeed(blockSlowAmount);
            isBlocking = value;
        }

   }

    void Damage()
    {
        noDamageTimer += Time.deltaTime;
        if (noDamageTimer > TimeInvulnerable)
        {
            _pSystem.Stop();
            isDamaged = false;
            noDamageTimer = 0;
            OnUpdate -= Damage;
        }
    }
}
