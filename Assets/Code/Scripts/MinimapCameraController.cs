using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform currentBoundary;

    public Camera camera;

    // True when the map is fullscreen
    private bool MapLarge;

    // The float for the zoom of the camera used for fullseceen map
    private float toZoom;



    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            transform.position = new Vector3(-67.49f, 35.97f, -8.29f);
        }
        else
        { 
            transform.position = new Vector3(data.MMPos[0], data.MMPos[1], -8.29f);
        }

        MapLarge = false;
        toZoom = 26;
    }

    void Update()
    {
        transform.position = new Vector3(currentBoundary.position.x, currentBoundary.position.y, -8.29f);

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(MapLarge == true)
            {
                toZoom = 26;
                MapLarge = false;
            }
            else if(MapLarge == false)
            {
                toZoom = 50;
                MapLarge = true;
            }
        }

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, toZoom, 2 * Time.deltaTime);
    }

    public void UpdateBoundary(GameObject newBoundary, GameObject oldBoundary)
    {
        if (oldBoundary) { oldBoundary.GetComponent<Boundary>().inRoom = false; }

        if (newBoundary) { newBoundary.GetComponent<Boundary>().inRoom = true; }
    }
}
