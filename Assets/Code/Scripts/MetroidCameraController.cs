using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroidCameraController : MonoBehaviour
{
    public enum RoomZone { Tutorial, Hydroponics, Mechanical, Cockpit, Other };

    // Transform of the player
    private Transform player;

    // The boundry "room" the player is currently in (set by PlayerCharacterController)
    public GameObject currentBoundary;

    // Current Boundary variables
    // CB stands for Current Boundary
    private Vector3 CBPos;
    private Vector3 CBExtents;

    // The speed that the camera can move
    private float CamSpeed = 0.05f;

    public float toSize;

    public float xLimit;
    public float yLimit;
    private float zLimit;




    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            transform.position = new Vector3(-55.8f, -7.1f, -8);

            Debug.Log("Camera position was set to default");
        }
        else
        {
            transform.position = new Vector3(data.camPos[0], data.camPos[1], data.camPos[2]);
        }
    }

    
    private void Update()
    {
        if(currentBoundary.CompareTag("Bound")) { xLimit = 10f; yLimit = 5.5f; zLimit = 8; toSize = 6.25f; }
        else { xLimit = 9.7f; yLimit = 5; zLimit = 8f; toSize = 6; }

        CBPos = currentBoundary.gameObject.transform.position;
        CBExtents = currentBoundary.GetComponent<Transform>().localScale / 2;

        transform.position = new Vector3(
            Mathf.Lerp(
                transform.position.x, 
                Mathf.Clamp(
                    player.position.x, 
                    CBPos.x - (CBExtents.x - xLimit), 
                    CBPos.x + (CBExtents.x - xLimit)
                    ), 
                CamSpeed), 
            Mathf.Lerp(
                transform.position.y, 
                Mathf.Clamp(
                    player.position.y, 
                    CBPos.y - (CBExtents.y - yLimit), 
                    CBPos.y + (CBExtents.y - yLimit)
                    ), 
                CamSpeed), 
            -zLimit
            );

        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(gameObject.GetComponent<Camera>().orthographicSize, toSize, CamSpeed);
    }

}
