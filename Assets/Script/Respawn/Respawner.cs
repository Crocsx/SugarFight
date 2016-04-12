using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {

    // EVENTS -----------------------------------------------------



    // PROPERTIES -----------------------------------------------------

    Transform   _transform;
    Transform   _playerRef;
    Vector3     _sPosition;

    [HideInInspector]
    public bool isAvailable = true;

    public float timeRespawn = 3.0f;


    // INTERFACE -----------------------------------------------------

    void Start()
    {
        _transform = transform;
        _sPosition = _transform.position;
        EventManager.StartListening("OnStageStart", startSpawn);
    }

    // METHODS -----------------------------------------------------

    public void AddPlayer(Transform player)
    {
        _playerRef = player;
        AttachPlayer();
    }

    public void startSpawn()
    {
        if(_playerRef != null)
            StartCoroutine("Spawn");
    }

    public void AskRespawn(Transform player)
    {
        AddPlayer(player);
        startSpawn();
    }

    public void AttachPlayer()
    {
        _playerRef.GetComponent<PlayerController>().enabled = false;
        _playerRef.position = transform.position;
        _playerRef.parent = _transform;
        isAvailable = false;
    }

    public void DetachPlayer()
    {
        _playerRef.GetComponent<PlayerController>().enabled = true;
        _transform.position = _sPosition;
        _playerRef.parent = null;
        isAvailable = true;
    }

    IEnumerator Spawn()
    {
        AttachPlayer();

        Vector3 targetPos = _transform.position+ Vector3.down;

        float currTime = 0.0f;

        while(timeRespawn > currTime)
        {
            if(currTime < timeRespawn * 0.5f)
            {
                _transform.position = Vector3.Lerp(_sPosition, targetPos, (currTime / (timeRespawn * 0.5f)));
            }
            currTime += Time.deltaTime;
            yield return null;
        }

        DetachPlayer();
   }
}
