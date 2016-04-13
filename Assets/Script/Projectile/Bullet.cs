using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed = 5;
    public float lifeTime = 1.0f;
    public float pushDamage;

    [HideInInspector]
    public Transform launcher;

    float timer = 0;
    Vector3 dir;
    Transform _transform;
	// Use this for initialization
	void Start () {
        _transform = transform;
        dir = Vector3.Scale(launcher.right, launcher.localScale);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        _transform.position += -(dir * speed) * Time.deltaTime;

        if (timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && collider != launcher)
        {
            collider.GetComponent<PlayerFighter>().OnDamaged(pushDamage, transform);
        }

        if(!collider.CompareTag("Player"))
            Destroy(gameObject);
    }
}
