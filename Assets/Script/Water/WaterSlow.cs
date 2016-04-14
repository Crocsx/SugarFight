using UnityEngine;
using System.Collections;

public class WaterSlow : MonoBehaviour {
    [Range(0, 1)]
    public float reduction;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerControl>().ReduceSpeed(reduction);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerControl>().AugmentSpeed(reduction);
        }
    }
}
