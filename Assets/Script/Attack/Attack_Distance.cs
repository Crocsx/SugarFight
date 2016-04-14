using UnityEngine;
using System.Collections;

public class Attack_Distance : Attack {

    public GameObject Projectile;
    public float timeCharge;

    bool _spawned;

    public override void OnUpdate()
    {
        if (isAttackPlaying)
        {
            if(!_spawned && attackTimer > timeCharge)
            {
                GameObject pjt = Instantiate(Projectile, _owner.position - Vector3.Scale(_owner.right, _owner.localScale), Quaternion.identity) as GameObject;
                pjt.transform.GetComponent<Bullet>().launcher = _owner;
                _spawned = true;
            }
            if (attackTimer > attackRecovery)
            {
                Stop();
            }
            attackTimer += Time.deltaTime;
        }
    }

    public override void Interrupt()
    {
        Stop();
    }

    public override void Start()
    {
        _spawned = false;
        attackTimer = 0;
        base.Start();
    }

    public override void Stop()
    {
        _spawned = true;
        attackTimer = 0;
        base.Stop();
    }
}
