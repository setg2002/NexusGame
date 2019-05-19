using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public MetroidCameraController.RoomZone ThisRoomZone;

    private Color throughColor;

    // True when the player has walked through this boundary
    public bool through;

    public int roomNum;

    public bool inRoom;

    // Floats for changing the color of minimap room
    float speed = 1f;
    float t;



    private void Awake()
    {
        int num;
        string str;
        str = gameObject.transform.name;
        str = str.Substring(str.Length - 2, 2);
        int.TryParse(str, out num);
        roomNum = num;

        PlayerData data = SaveSystem.LoadPlayer();

        if(data == null)
        {
            through = false;
            inRoom = false;
        }
        else
        {
            through = data.MMRoomColor[roomNum];
        }

        if (ThisRoomZone == MetroidCameraController.RoomZone.Tutorial) { throughColor = new Color(0.3f, 0.4f, 1); }
        if (ThisRoomZone == MetroidCameraController.RoomZone.Hydroponics) { throughColor = new Color(0.7f, 0.5f, 0.3f); }
        if (ThisRoomZone == MetroidCameraController.RoomZone.Mechanical) { throughColor = new Color(0.8f, 0.8f, 0.1f); }
        if (ThisRoomZone == MetroidCameraController.RoomZone.Cockpit) { throughColor = new Color(0.6f, 1, 0); }
        if (ThisRoomZone == MetroidCameraController.RoomZone.Other) { throughColor = new Color(0.8f, 0, 1); }


    }

    private void Update()
    {
        if(inRoom == true)
        {
            // Changing color of text
            t = Mathf.PingPong(Time.time * speed, 2.0f);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(0.7f, 0.7f, 0.7f, 1), Mathf.PingPong(Time.time * speed, 1));
        }
        else if(through == true)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = throughColor;
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.15f, 0.15f, 0.15f);
        }
    }
}
