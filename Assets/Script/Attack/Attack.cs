using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attack : MonoBehaviour, iAttack
{
    public DamageHitbox script;
    public float pushDamage;

    public string animName;
    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public int timePressed;
    [HideInInspector]
    public float attackRate = 0.3f;
    [HideInInspector]
    public event onAttackEnd OnAttackEnd;

    protected bool isAttackPlaying;

    public virtual void OnUpdate()
    {

    }

    public virtual void Start()
    {
        timePressed = 0;
        attackTimer = 0;
        isAttackPlaying = true;
    }

    public virtual void Stop()
    {
        isAttackPlaying = false;
        OnAttackEnd();
    }
}
