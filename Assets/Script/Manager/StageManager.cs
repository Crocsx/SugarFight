using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class playerReference
{
    public GameObject reference;
    public int lifeRemaining;
}

public class StageManager : MonoBehaviour {


    // EVENTS -----------------------------------------------------

    public delegate void OnPlayerSpawnDelegate(GameObject player);
    public OnPlayerSpawnDelegate OnPlayerSpawn;

    // PROPERTIES --------------------------------------------------------

    public int nbPlayer = 2;
    public int nbLife = 4;

    public GameObject playerPrefab;
    [HideInInspector]
    public Dictionary<string, playerReference> lifeRemaining = new Dictionary<string, playerReference>();
    [HideInInspector]
    public int loser;

    public Color[] PlayerColor = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta, Color.grey, Color.black };
    // INTERFACE -----------------------------------------------------

    void Awake ()
    {
        EventManager.StartListening("OnPause", Pause);
        EventManager.StartListening("UnPause", unPause);
        EventManager.StartListening("OnCinematicFinished", SpawnPlayers);
    }

    void SpawnPlayers()
    {
        for (var i = 0; i < nbPlayer; i++)
        {
            Respawner _respawner = FindAvailableRespawner();
            GameObject nPlayer = SpawnPlayer(playerPrefab, i, PlayerColor[i]);
            _respawner.AddPlayer(nPlayer.transform);
        }

        EventManager.TriggerEvent("OnStageStart");
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    void unPause()
    {
        Time.timeScale = 1;
    }

    GameObject SpawnPlayer(GameObject prefab, int id, Color color)
    {
        GameObject nPlayer = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
        PlayerHandler pHandler = nPlayer.GetComponent<PlayerHandler>();
        pHandler.Setup(id, color);
        pHandler.OnDeath += PlayerDead;

        playerReference newPlayer = new playerReference();
        newPlayer.reference = nPlayer;
        newPlayer.lifeRemaining = nbLife;

        lifeRemaining.Add(("Player" + id), newPlayer);

        if (OnPlayerSpawn != null)
            OnPlayerSpawn(nPlayer);

        return nPlayer;
    }

    void PlayerDead (string pName, Transform transform)
    {
        if (lifeRemaining.ContainsKey(pName))
        {
            --lifeRemaining[pName].lifeRemaining;
        }
        if(lifeRemaining[pName].lifeRemaining > 0)
        {
            Respawner _respawner = FindAvailableRespawner();
            _respawner.AskRespawn(transform);
        }
        else
        {
            loser = lifeRemaining[pName].reference.GetComponent<PlayerHandler>().id;
            EventManager.TriggerEvent("EndGame");
            StartCoroutine("EndGame");
        }
        EventManager.TriggerEvent("PlayerDeath");
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    // METHODS --------------------------------------------------------

    Respawner FindAvailableRespawner()
    {
        Respawner[] _respawner = GameObject.FindObjectsOfType<Respawner>();
        for (var i = 0; i < _respawner.Length; i++)
        {
            if (_respawner[i].isAvailable)
            {
                return _respawner[i];
            }
        }
        return null;
    }
}
