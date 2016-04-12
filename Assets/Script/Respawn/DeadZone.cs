using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Transform _entity = other.transform;

        if (_entity.CompareTag("Player"))
            _entity.GetComponent<PlayerController>().askDie();
    }
}