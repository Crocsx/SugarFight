using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingCamera : MonoBehaviour {

    float minX  = 9999999;
    float maxX = -9999999;
    float minY  = 9999999;
    float maxY  = -9999999;
    float camSpeed = 5.0f;
    float camDist = 8;
    Vector3 finalLookAt;
    Vector3 cameraBuffer;


    void Update()
    {

        CalculateBounds();

        CalculateCameraPosAndSize();

    }

    void CalculateBounds()
    {
        minX = Mathf.Infinity; maxX = -Mathf.Infinity; minY = Mathf.Infinity; maxY = -Mathf.Infinity;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Vector3 tempPlayer = player.transform.position;
            //X Bounds
            if (tempPlayer.x < minX)
                minX = tempPlayer.x;
            if (tempPlayer.x > maxX)
                maxX = tempPlayer.x;
            //Y Bounds
            if (tempPlayer.y < minY)
                minY = tempPlayer.y;
            if (tempPlayer.y > maxY)
                maxY = tempPlayer.y;
        }
    }

    void CalculateCameraPosAndSize()
    {
        Vector3 cameraCenter = Vector3.zero;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            cameraCenter += player.transform.position;
        }
        Vector3 finalCameraCenter = cameraCenter / players.Length;
        //Rotates and Positions camera around a point
        Quaternion rot = Quaternion.Euler(new Vector3(45,0,0));
        Vector3 pos = rot * new Vector3(0f, 0f, -camDist) + finalCameraCenter;
        transform.rotation = rot;
        transform.position = Vector3.Lerp(transform.position, pos, camSpeed * Time.deltaTime);
        finalLookAt = Vector3.Lerp(finalLookAt, finalCameraCenter, camSpeed * Time.deltaTime);
        transform.LookAt(finalLookAt);
        //Size
        float sizeX = maxX - minX + cameraBuffer.x;
        float sizeY = maxY - minY + cameraBuffer.y;
        float camSize = (sizeX > sizeY ? sizeX : sizeY);
        transform.GetComponent<Camera>().orthographicSize = camSize * 0.5f;
    }
}
