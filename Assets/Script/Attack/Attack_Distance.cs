using UnityEngine;
using System.Collections;

public class Attack_Distance : Attack {

    public GameObject Projectile;
    public float timeCharge;

    bool _spawned;
    float _ratio;

    public override void OnUpdate()
    {
        if (isAttackPlaying)
        {
            _ratio += Time.deltaTime;
            if (!_spawned && attackTimer > timeCharge)
            {
                Animator _anim = _owner.transform.GetChild(0).GetComponent<Animator>();
                _anim.SetBool("SpitCharge", false);
                _anim.SetBool("Spit", true);
                Spawn();
            }
            if (attackTimer > attackRecovery)
            {
                Animator _anim = _owner.transform.GetChild(0).GetComponent<Animator>();
                _anim.SetBool("Spit", false);
                Stop();
            }
            attackTimer += Time.deltaTime;
        }
    }
    void Spawn()
    {
        GameObject pjt = Instantiate(Projectile, _owner.position + (Vector3.Scale(_owner.right, _owner.localScale)* 2), Quaternion.identity) as GameObject;
        pjt.transform.GetComponent<Bullet>().Launch(_owner, (_ratio / timeCharge));
        _spawned = true;
        attackTimer = timeCharge;
    }

    public override void Interrupt()
    {
        if(!_spawned)
            Spawn();
    }

    public override void Start()
    {
        _ratio = 0;
        _spawned = false;
        attackTimer = 0;
        base.Start();
    }

    public override void Stop()
    {
        _ratio = 0;
        _spawned = false;
        attackTimer = 0;
        base.Stop();
    }
}
