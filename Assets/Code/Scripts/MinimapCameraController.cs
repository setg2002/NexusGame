using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform currentBoundary;

    public Camera camera;

    private bool reactivateAudio = false;
    private bool reactivateItem = false;
    private bool reactivateUse = false;

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

        if(MapLarge == true)
        {
            if (GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.activeSelf)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(false);
                reactivateAudio = true;
            }
            else { reactivateAudio = false; }
            if (GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.activeSelf)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.SetActive(false);
                reactivateUse = true;
            }
            else { reactivateUse = false; }
            if (GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.activeSelf)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.SetActive(false);
                reactivateItem = true;
            }
            else { reactivateItem = false; }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(MapLarge == true)
            {
                toZoom = 26;
                MapLarge = false;

                if (reactivateAudio)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(true);
                    reactivateAudio = false;
                }
                if (reactivateItem)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.SetActive(true);
                    reactivateItem = false;
                }
                if (reactivateUse)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.SetActive(true);
                    reactivateUse = false;
                }
            }
            else if(MapLarge == false)
            {
                toZoom = 50;
                MapLarge = true;

                if (GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(false);
                    reactivateAudio = true;
                }
                if (GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.SetActive(false);
                    reactivateUse = true;
                }
                if (GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.activeSelf)
                {
                    GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.SetActive(false);
                    reactivateItem = true;
                }
            }
        }

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, toZoom, 8 * Time.deltaTime);
    }

    public void UpdateBoundary(GameObject newBoundary, GameObject oldBoundary)
    {
        if (oldBoundary) { oldBoundary.GetComponent<Boundary>().inRoom = false; }

        if (newBoundary) { newBoundary.GetComponent<Boundary>().inRoom = true; }
    }
}
