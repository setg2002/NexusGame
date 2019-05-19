using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform currentBoundary;



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
    }

    void Update()
    {
        transform.position = new Vector3(currentBoundary.position.x, currentBoundary.position.y, -8.29f);
    }

    public void UpdateBoundary(GameObject newBoundary, GameObject oldBoundary)
    {
        if (oldBoundary) { oldBoundary.GetComponent<Boundary>().inRoom = false; }

        if (newBoundary) { newBoundary.GetComponent<Boundary>().inRoom = true; }
    }
}
