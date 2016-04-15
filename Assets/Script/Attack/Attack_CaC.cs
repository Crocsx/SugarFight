using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attack_CaC : Attack {
    public DamageHitbox hitboxHandler;
    public float pushDamage;

    public override void Setup(Transform owner)
    {
        base.Setup(owner);
        hitboxHandler.Setup(pushDamage, _owner);
    }

    public override void OnUpdate()
    {
        if (isAttackPlaying)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackRecovery || timePressed >= 4)
            {
                Stop();
            }
        }
    }
}
