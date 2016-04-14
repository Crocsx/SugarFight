using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float maxSpeed = 5;
    public float maxLifeTime = 1.0f;
    public float maxDamage;

    Transform _launcher;

    float _timer = 0;
    float _lifeTime = 0;
    float _speed = 0;
    float _dmg = 0;
    Vector3 _dir;
    Transform _transform;

	void Start () {
        _transform = transform;
    }

    public void Launch(Transform launcher, float ratio)
    {
        _launcher = launcher;
        _lifeTime = ratio * maxLifeTime;
        _speed = ratio * maxSpeed;
        _dmg = ratio * maxDamage;
        _dir = Vector3.Scale(-_launcher.right, launcher.localScale);
    }
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime;

        _transform.position += -(_dir * _speed) * Time.deltaTime;

        if (_timer > _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && collider != _launcher)
        {
            collider.GetComponent<PlayerFighter>().OnDamaged(_dmg, transform);
        }

        if(!collider.CompareTag("Player"))
            Destroy(gameObject);
    }
}
