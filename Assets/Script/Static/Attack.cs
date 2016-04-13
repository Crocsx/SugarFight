using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Attack {
    public float pushDamage;
    public DamageHitbox script;
    public string animName;
    [HideInInspector]
    public bool attack;
    [HideInInspector]
    public float attackTimer;
    [HideInInspector]
    public int timePressed;
   
}
