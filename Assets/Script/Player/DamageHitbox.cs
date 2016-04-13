using UnityEngine;
using System.Collections;

public class DamageHitbox : MonoBehaviour {

    Transform owner;
    float _damage;

    public void Setup (Attack _attack, Transform _owner) {
        owner = _owner;
        _damage = _attack.pushDamage;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && collider != owner)
        {
            collider.GetComponent<PlayerFigher>().OnDamaged(_damage, owner);
        }
    }
}
