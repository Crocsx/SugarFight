using UnityEngine;
using System.Collections;

public delegate void onAttackEnd();

public interface iAttack {

    event onAttackEnd OnAttackEnd;

    void Setup(Transform value);

    void OnUpdate();

    void Start();

    void Stop();

    void Interrupt();
}
