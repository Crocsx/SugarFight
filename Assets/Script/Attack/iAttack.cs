using UnityEngine;
using System.Collections;

public delegate void onAttackEnd();

public interface iAttack {

    event onAttackEnd OnAttackEnd;

    void OnUpdate();

    void Start();

    void Stop();
	
}
