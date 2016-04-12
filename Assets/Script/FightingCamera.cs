using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCamera : MonoBehaviour {

    // Expecting 2 players only
    public List<GameObject> players = new List<GameObject>();
    public Camera mainCamera;

    public const int CENTER_TO_CAMERA = 10; // temporary
    public const float ANGLE = 45; // degree

    //GameObject clone;

    // Use this for initialization
    void Start () {

        Vector3 p1 = players[0].transform.position; // A
        Vector3 p2 = players[1].transform.position; // B
        Vector3 center = (p1 + p2) / 2;             // D
        float distance = Vector3.Distance(p1, p2); // |AB|
        float centerToCamera = Vector3.Distance(p1, p2);

        Vector3 p1ToP2 = p2 - p1; // AB
        Vector3 h = p1ToP2;
        h.Set(p1ToP2.x, -p1ToP2.y, 0);

        float xAndYFromCenterToCamera = CENTER_TO_CAMERA * Mathf.Cos(ANGLE * Mathf.PI / 180); // que si 45 degrée... bof

        Vector3 forCamera = center;
        Debug.Log(xAndYFromCenterToCamera);
        forCamera.x += xAndYFromCenterToCamera; // rotation sol
        forCamera.y += xAndYFromCenterToCamera; // hauteur
        forCamera.z += xAndYFromCenterToCamera;


        transform.position = forCamera;

        transform.LookAt(center);

        CreateClone(center);

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 p1 = players[0].transform.position; // A
        Vector3 p2 = players[1].transform.position; // B
        Vector3 center = (p1 + p2) / 2;             // D
        float distance = Vector3.Distance(p1, p2); // |AB|
        float centerToCamera = Vector3.Distance(p1, p2);

        Vector3 p1ToP2 = p2 - p1; // AB
        Vector3 h = p1ToP2;
        h.Set(p1ToP2.x, -p1ToP2.y, 0);

        float xAndYFromCenterToCamera = CENTER_TO_CAMERA * Mathf.Cos(ANGLE * Mathf.PI / 180); // que si 45 degrée... bof

        Vector3 forCamera = center;
        Debug.Log(xAndYFromCenterToCamera);
        forCamera.x += xAndYFromCenterToCamera; // rotation sol
        forCamera.y += xAndYFromCenterToCamera; // hauteur
        forCamera.z += xAndYFromCenterToCamera;

        Debug.DrawLine(center, forCamera, new Color(0.2f,0.5f,0.7f));


        transform.position = forCamera;
        //clone.transform.position = forCamera;

        transform.LookAt(center);

    }


    void CreateClone(Vector3 myPosition)
    {
        GameObject clone;
        clone = Instantiate(players[0].transform,
                            myPosition,
                            players[0].transform.rotation) as GameObject;
    }
}
