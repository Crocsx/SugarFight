using UnityEngine;
using System.Collections;

public class DamageHitbox : MonoBehaviour {

    Transform owner;
    float _damage;

    public void Setup (float dmg, Transform _owner) {
        owner = _owner;
        _damage = dmg;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && collider != owner)
        {
            collider.GetComponent<PlayerFighter>().OnDamaged(_damage, owner);
        }
    }
}
