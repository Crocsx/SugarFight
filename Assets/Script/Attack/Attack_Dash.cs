using UnityEngine;
using System.Collections;

public class Attack_Dash : Attack {
    public float timeCharge;
    public float power;
    bool _asDashed;

    public override void OnUpdate()
    {
        if (isAttackPlaying)
        {
            if (!_asDashed && attackTimer > timeCharge)
            {
                Vector2 axe = _owner.GetComponent<PlayerInputs>()._currAxis;
                Vector3 dir = new Vector3(axe.y, 0, axe.x);
                _owner.GetComponent<Rigidbody>().AddForce(power * dir * 100);
                _asDashed = true;
            }
            if (attackTimer > attackRecovery)
            {
                Stop();
            }
            attackTimer += Time.deltaTime;
        }
    }

    public override void Start()
    {
        _asDashed = false;
        attackTimer = 0;
        base.Start();
    }

    public override void Stop()
    {
        _asDashed = true;
        attackTimer = 0;
        base.Stop();
    }

    public override void Interrupt()
    {
        if(!_asDashed)
            Stop();
    }
}
