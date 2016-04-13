using UnityEngine;
using System.Collections;

[System.Serializable]
public class DefaultAttack : Attack {
    public override void OnUpdate()
    {
        if (isAttackPlaying)
        {
            attackTimer += Time.fixedDeltaTime;

            if (attackTimer > attackRate || timePressed >= 4)
            {
                Stop();
            }
        }
    }
}
