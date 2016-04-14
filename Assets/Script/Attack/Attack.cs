using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attack : MonoBehaviour, iAttack
{
    public float attackRecovery = 0.3f;
    public string animName;

    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public int timePressed;
    [HideInInspector]
    public event onAttackEnd OnAttackEnd;
    [HideInInspector]
    public bool isAttackPlaying;

    protected Transform _owner;

    public virtual void OnUpdate()
    {

    }

    public virtual void Setup(Transform owner)
    {
        _owner = owner;
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

    public virtual void Interrupt()
    {

    }
}
