using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCamera : MonoBehaviour {

    // Expecting 2 players only
    public List<GameObject> players = new List<GameObject>();

    public const int CENTER_TO_CAMERA = 10; // temporary
    public const float ANGLE_VIEW = 45; // degree
    public const float ANGLE_PLONGE = 45; // degree 0 à 90
    public const float SMOOTH = 14;
    public const float MIN_DISTANCE = 5;
    //public const float CAMERA_HEIGHT = 7;
    //public Vector3 cameraPos = new Vector3(10,10,10);
    public Camera myCamera;

    public GameObject DERBUG;

    int playersLength = 0;
    Vector3 centerBefore = new Vector3();
    Vector3 cameraPosBefore = new Vector3();
    float centerToCameraBefore = 0;
    Vector3 center = new Vector3();

    //GameObject clone;

    // Use this for initialization
    void Start () {

        myCamera.fieldOfView = ANGLE_VIEW;
        myCamera.transform.position = new Vector3(myCamera.transform.position.x, myCamera.transform.position.y, 0);

        playersLength = players.Count;

        updateCenter();
        centerBefore = center;
        cameraPosBefore = myCamera.transform.position;

        //CreateClone(center);

        //myCamera.transform.Rotate(Vector3.right * ANGLE_PLONGE); redondant avec look at


        myCamera.transform.LookAt(center);

        centerToCameraBefore = Vector3.Magnitude(myCamera.transform.position - center);




        /*
        Vector3 p1 = players[0].transform.position; // A
        Vector3 p2 = players[1].transform.position; // B
        Vector3 center = (p1 + p2) / 2;             // D
        float distance = Vector3.Distance(p1, p2); // |AB|
        float centerToCamera = Vector3.Distance(p1, p2);

        Vector3 p1ToP2 = p2 - p1; // AB
        Vector3 h = p1ToP2;
        h.Set(p1ToP2.x, -p1ToP2.y, p1ToP2.z);

        Vector3 hUnitaire = Vector3.Normalize(h);
        Debug.Log(hUnitaire);


        float xAndYFromCenterToCamera = CENTER_TO_CAMERA * Mathf.Cos(ANGLE * Mathf.PI / 180); // que si 45 degrée... bof





        // 11 04 2016
        Vector3 forCamera = center;
        Debug.Log(xAndYFromCenterToCamera);
        forCamera.x += xAndYFromCenterToCamera; // rotation sol
        forCamera.y += xAndYFromCenterToCamera; // hauteur
        forCamera.z += xAndYFromCenterToCamera;


        transform.position = forCamera;

        transform.LookAt(center);

        CreateClone(center);
        */
    }
	
	// Update is called once per frame
	void Update () {

        // obtenir les dépassements de l'écran en % de taille du viewport
        const float HALF_CAMERA = 0.5f;
        const float MARGIN = 0.25f;
        float pourcentOutHorizontal = 0f; 
        float pourcentOutVertical = 0f;
        Vector3 farestPlayerHorizontal = new Vector3();
        Vector3 farestPlayerVertical = new Vector3();



        updateCenter();
        //LookTo(center, myCamera);

        /*
        float testCenter = 0;
        for (int i = 0; i < playersLength; i++)
        {
            testCenter += players[i].transform.position;
        }
        testCenter /= playersLength;*/


        MOVEDEBUG(center);


        myCamera.transform.position += center - centerBefore; //duplique les mvts de la camera

        myCamera.transform.LookAt(center); // focus centre.



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
        
        //Debug.Log(farestPlayerHorizontal); // 0.5 0.5 x.x // maintenant c'est règlé de 0.0 à plus
        // si 0.1 alors il faut zoomIn de 90% (agrandir)
        // si 1.1 alros ils faut zoomOut de 10%
        // souvent entre 0.5 et 1+
        // multiplié par deux
        pourcentOutHorizontal = Mathf.Abs(farestPlayerHorizontal.x);
        pourcentOutVertical   = Mathf.Abs(farestPlayerVertical.y);


        float zoomChange = pourcentOutHorizontal > pourcentOutVertical ? pourcentOutHorizontal : pourcentOutVertical;
        //Debug.Log(pourcentOutHorizontal > pourcentOutVertical ? "pourcentOutHorizontal" : "pourcentOutVertical;"); // pb pas lié à cela
        zoomChange *= 2;
        zoomChange += MARGIN;

        //Debug.Log(pourcentOutHorizontal); // 0 
        float newCenterToCamera = Vector3.Magnitude(myCamera.transform.position - center); // distance caméra centre actuel
        //Vector3 unitaireCenterToCamera = Vector3.Normalize(myCamera.transform.position - center);
        //Debug.Log(centerToCamera);
        //Debug.Log(zoomChange);
        //Debug.Log(newCenterToCamera);
        newCenterToCamera *= zoomChange;


        if (newCenterToCamera > MIN_DISTANCE)
        {
            //Vector3 vec3ZoomChange = -myCamera.transform.forward * (newCenterToCamera - centerToCameraBefore); // vecteur de changement // bon
            Vector3 vec3ZoomChange = myCamera.transform.forward * (centerToCameraBefore - newCenterToCamera); // vecteur de changement // bon
            myCamera.transform.position += vec3ZoomChange;
            //myCamera.transform.position = -myCamera.transform.forward * newCenterToCamera; // écrase les autres 12/04 // si draw line part n'impote où...

            //Debug.DrawLine(-myCamera.transform.forward * newCenterToCamera, -myCamera.transform.forward * centerToCameraBefore); // zoomChange est bon
            Debug.DrawLine(myCamera.transform.position, myCamera.transform.position + vec3ZoomChange); // zoomChange est bon
        }



        /* lookat unidirection Y
        Vector3 targetPostition = new Vector3(center.x,
                                        myCamera.transform.position.y,
                                        center.z);
        myCamera.transform.LookAt(targetPostition);
        */







        /*
         
      float newAngle = 0f;






      // 45 DEGREE uniquement (sinon cos et sinus à utiliser)
      // distance au sol entre camera et centre
      float temp = newCenterToCamera * Mathf.Cos((ANGLE_PLONGE * Mathf.PI) / 180);

      Vector3 newVec3 = new Vector3(temp * Mathf.Sin(newAngle), temp, temp * Mathf.Cos(newAngle));

      // rotation y seulement, z = 45, x = 0;
      // translation x et z et y


      // centerToCamera à plat, et lui faire un rotate around
      ////myCamera.transform.position = unitaireCenterToCamera * newCenterToCamera; // a plat
      ////myCamera.transform.RotateAround(center, Vector3.forward, ANGLE_PLONGE); // ANGLE_PLONGE


      //Vector3 newVec3 = new Vector3(temp, temp, myCamera.transform.position.z);

      newVec3 += (centerBefore - center);
      myCamera.transform.position += (newVec3 - myCamera.transform.position) / SMOOTH; // newPos

      // si deux players je tourne autour en prenant la perpendiculaire, sinon rien.
      // càd vector 3 normalize pour avoir l'angle en y, aplliqué ensuite à la distance x et z de la caméra.
      if (playersLength == 2)
      {

      }
      // temp camera tourne tout seule.
      //myCamera.transform.RotateAround(center, Vector3.up, 20 * Time.deltaTime);

      // vérification (debug)
      for (int i = 0; i < playersLength; i++)
      {
          //Debug.Log(isVisible(players[i].transform.position));
      }
      */
        centerToCameraBefore = newCenterToCamera;
        centerBefore = center;
    }

    void updateCenter()
    {
        center = new Vector3() ;
        for (int i = 0; i < playersLength; i++)
        {
            center += players[i].transform.position;
        }
        center /= playersLength;
    }


    bool isVisible(Vector3 target)
    {
        Vector3 screenPoint = myCamera.WorldToViewportPoint(target);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            return true;
        }
        return false;
    }


    void CreateClone(Vector3 myPosition)
    {
        GameObject clone;
        clone = Instantiate(players[0].transform,
                            myPosition,
                            players[0].transform.rotation) as GameObject;
    }

    void MOVEDEBUG(Vector3 posa)
    {
        DERBUG.transform.position = posa;
    }

    void LookTo(Vector3 pTarget, Camera pCamera)
    {
        Vector3 relativePos = pTarget - pCamera.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        pCamera.transform.rotation = rotation;
    }
}
