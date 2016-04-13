/* Created by Rabier Ambroise 11/04/2016
 
Classe gérant la caméra, fixé par rapport à la moyenne des positions des joueurs (centre), zoom-dézoom de façon à avoir toujours les joeuurs à l'écran.
Utiliser MARGIN_ZOOM pour augmenter la distance de la caméra de façon constante (%/2).
Utiliser MIN_DISTANCE pour la distance minimale entre le centre et la caméra.
Positionnez la caméra à la main dans unity, seul le zoom et le lookAt changera.
 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCamera : MonoBehaviour {

    // Expecting 2 players only
    public List<GameObject> players = new List<GameObject>();

    public float ANGLE_VIEW = 45; // degree
    public float SMOOTH = 14;
    public float MIN_DISTANCE = 5;
    public float MARGIN_ZOOM = 0.25f;
    public StageManager _sManager;
    public GameObject Stage;

    Camera      myCamera;
    Transform   _transform;
    int         playersLength = 0;
    float       centerToCameraBefore = 0;
    Vector3     centerBefore;
    Vector3     cameraPosBefore;
    Vector3     center;

    void Awake ()
    {
        _transform = transform;
        myCamera = _transform.GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        center = Vector3.zero;
        centerBefore = Vector3.zero;
        cameraPosBefore = _transform.position;
        centerToCameraBefore = Vector3.Magnitude(_transform.position - center);

        players.Clear();
        players.Add(Stage);

        EventManager.StartListening("OnStageStart", startSpawn);

        myCamera.fieldOfView = ANGLE_VIEW;
        _sManager.OnPlayerSpawn += AddPlayer;
    }

    void startSpawn()
    {
        updateCenter();
        _transform.LookAt(center);
    }

    // Permet d'ajouter les players que la caméra suit par code
    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        playersLength = players.Count;
    }

    // Update is called once per frame
    void Update () {

        updateCenter();

        _transform.position += center - centerBefore; //duplique les mvts de la camera

        _transform.LookAt(center); // focus centre.

        CorrectZoom();

        centerBefore = center;
    }

    // corrige le zoom
    void CorrectZoom()
    {
        const float HALF_CAMERA = 0.5f;
        float pourcentOutHorizontal = 0f;
        float pourcentOutVertical = 0f;
        Vector3 farestPlayerHorizontal = new Vector3();
        Vector3 farestPlayerVertical = new Vector3();


        // obtenir les dépassements de l'écran en % de taille du viewport
        for (int i = 0; i < playersLength; i++)
        {
            // calcul de la distance la plus éloigné, en vertical ainsi qu'en horizontale
            Vector3 distance = myCamera.WorldToViewportPoint(players[i].transform.position);
            distance -= new Vector3(HALF_CAMERA, HALF_CAMERA, -distance.z); // position à partir du centre de l'écran


            if (Mathf.Abs(distance.x) > Mathf.Abs(farestPlayerHorizontal.x))
            {
                farestPlayerHorizontal = distance;
            }

            if (Mathf.Abs(distance.y) > Mathf.Abs(farestPlayerVertical.y))
            {
                farestPlayerVertical = distance;
            }

        }

        // si 0.1 alors il faut zoomIn de 90% (agrandir)
        // si 1.1 alros ils faut zoomOut de 10%
        // souvent entre 0.5 et 1+
        // multiplié par deux
        pourcentOutHorizontal = Mathf.Abs(farestPlayerHorizontal.x);
        pourcentOutVertical = Mathf.Abs(farestPlayerVertical.y);

        // définit la priorité horizontale ou verticale
        float zoomChange = pourcentOutHorizontal > pourcentOutVertical ? pourcentOutHorizontal : pourcentOutVertical;
        zoomChange *= 2;
        zoomChange += MARGIN_ZOOM;

        // distance caméra centre actuel
        float newCenterToCamera = Vector3.Magnitude(_transform.position - center);
        newCenterToCamera *= zoomChange;


        if (newCenterToCamera > MIN_DISTANCE)
        {
            Vector3 vec3ZoomChange = transform.forward * (centerToCameraBefore - newCenterToCamera); // vecteur de changement
            _transform.position += vec3ZoomChange;
        }

        centerToCameraBefore = newCenterToCamera;
    }

    void updateCenter()
    {
        center = Vector3.zero;
        for (int i = 0; i < playersLength; i++)
        {
            center += players[i].transform.position;
        }
        center /= playersLength;
    }
}
