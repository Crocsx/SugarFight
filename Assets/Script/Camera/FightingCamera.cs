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

    public GameObject initPosition;
    public float ANGLE_VIEW = 45; // degree
    public float SMOOTH = 150;
    public float MARGE_ZOOM = 0.2f;
    public float ZOOM_MIN_DISTANCE = 8f;
    public float MAX_DISTANCE_PLAYER_TO_STAGE = 30; // if > then is ignored by camera
    public StageManager _sManager;
    public GameObject stage;
    public bool active;

    Camera      myCamera;
    Transform   _transform;
    int         playersLength = 0;
    float       centerToCameraBefore = 0;
    Vector3     centerBefore;
    Vector3     center;
    //Vector3     cameraTargetPosition;
    List<int>   ignoredPlayers;
    Vector3     initToStagePosition;

    void Awake ()
    {
        _transform = transform;
        myCamera = _transform.GetComponent<Camera>();
        active = false;
        ignoredPlayers = new List<int>();
        players.Clear();

        EventManager.StartListening("OnStageStart", startSpawn);

        _sManager.OnPlayerSpawn += AddPlayer;
    }

    // Use this for initialization
    void Start () {
        //initPosition.transform.position = new Vector3(initPosition.transform.position.x, -initPosition.transform.position.y, initPosition.transform.position.z);
        //_transform.position = initPosition.transform.position != null ? -initPosition.transform.position : _transform.position;

        center = stage.transform.position;
        centerBefore = stage.transform.position;
        initToStagePosition = initPosition.transform.position - center;

        myCamera.fieldOfView = ANGLE_VIEW;
    }

    void startSpawn()
    {
        // semble être inutile
        //updateCenter();
        //_transform.LookAt(center);
    }

    // Permet d'ajouter les players que la caméra suit par code
    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        playersLength = players.Count;
    }

    // Update is called once per frame
    void Update () {
        if (!active)
            return;
        updateCenter();


        Vector3 cameraStartPos = _transform.position;
        Vector3 cameraEndPos;


        _transform.position = center + initToStagePosition;

        Debug.DrawLine(center, _transform.position, Color.red);
        _transform.LookAt(center); // attention a désactiver PathSplines !
        
        _transform.position += CorrectZoom();


        // Smooth
        cameraEndPos = _transform.position;
        _transform.position = cameraStartPos;
        _transform.position += Vec3Floor(cameraEndPos - cameraStartPos, 1000) / SMOOTH;

        centerBefore = center;
    }

    // corrige le zoom
    Vector3 CorrectZoom()
    {
        const float HALF_CAMERA = 0.5f;
        float pourcentOutHorizontal = 0f;
        float pourcentOutVertical = 0f;
        Vector3 farestPlayerHorizontal = new Vector3();
        Vector3 farestPlayerVertical = new Vector3();


        // obtenir les dépassements de l'écran en % de taille du viewport
        for (int i = 0; i < playersLength; i++)
        {
            bool ignore = false;
            foreach (int myInt in ignoredPlayers)
            {
                if (myInt == i) ignore = true;
            }
            if (ignore) continue;

            // calcul de la distance la plus éloigné, en vertical ainsi qu'en horizontale
            Vector3 distance = myCamera.WorldToViewportPoint(players[i].transform.position);
            distance -= new Vector3(HALF_CAMERA, HALF_CAMERA, -distance.z); // position à partir du centre de l'écran

            if (distance.z < 0) return Vector3.zero; // au cas où un joueur passe derrière la caméra (interdit), possible que si un joueur est trop éloigné du stage.

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
        pourcentOutHorizontal = Mathf.Abs(farestPlayerHorizontal.x)*2;
        pourcentOutVertical = Mathf.Abs(farestPlayerVertical.y)*2;

        // définit la priorité horizontale ou verticale
        float zoomChange = pourcentOutHorizontal > pourcentOutVertical ? pourcentOutHorizontal : pourcentOutVertical;

        // distance caméra centre actuel
        centerToCameraBefore = Vector3.Magnitude(_transform.position - center); // cameraTargetPosition ?
        float newCenterToCamera = centerToCameraBefore * (zoomChange + MARGE_ZOOM);

        // faire math.round pour éviter un très léger tremblement de zoom ?
        Vector3 vec3ZoomChange = transform.forward * (centerToCameraBefore - newCenterToCamera); // vecteur de changement

        if (newCenterToCamera < ZOOM_MIN_DISTANCE)
        {
            return transform.forward * (centerToCameraBefore - ZOOM_MIN_DISTANCE);
        }
        return vec3ZoomChange;
    }

    void updateCenter()
    {
        ignoredPlayers = new List<int>();
        center = Vector3.zero;
        float skippedPlayers = 0f;


        for (int i = 0; i < playersLength; i++)
        {
            if (Vector3.Magnitude(players[i].transform.position - stage.transform.position) > MAX_DISTANCE_PLAYER_TO_STAGE)
            {
                ignoredPlayers.Add(i);
                skippedPlayers++;
                continue;
            }
            center += players[i].transform.position;
        }

        Debug.DrawLine(players[0].transform.position, players[1].transform.position, Color.red);


        if (playersLength - skippedPlayers > 0)
        {
            center /= playersLength - skippedPlayers;
        } else {
            center = stage.transform.position;
            Debug.Log("all players ignored or no players, default focus is stage.transform.position");
        }
    }

    /*
    void LookTo(Vector3 pTarget, Transform pTransform)
    {
        Vector3 relativePos = pTarget - pTransform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        pTransform.rotation = rotation;
    }*/

    // réduit un peu les tremblement de la caméra
    Vector3 Vec3Floor (Vector3 vector, int length) {
        vector.x = Mathf.Floor(vector.x * length) / length;
        vector.y = Mathf.Floor(vector.y * length) / length;
        vector.z = Mathf.Floor(vector.z * length) / length;


        return vector;
    }
}
