using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int GameStage;
    public float[] position;
    public float[] camPos;
    public float[] MMPos;
    public bool flipX;
    public bool HasBlackBox;
    public bool[] logActive;
    public bool[] itemActive;
    public int logCount;
    public string currentItem;

    public bool[] MMRoomColor;

    private int f;
    private int g;
    private int r;

    public PlayerData (PlayerCharacterController player, MetroidCameraController cameraController, GameObject[] audioLogs, ItemSave[] items, Boundary[] rooms, GameObject minimap)
    {
        flipX = player.gameObject.GetComponent<SpriteRenderer>().flipX;
        logCount = player.logCount;
        GameStage = player.GameStage;
        HasBlackBox = player.HasBlackBox;
        currentItem = player.currentItem;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        camPos = new float[3];
        camPos[0] = cameraController.transform.position.x;
        camPos[1] = cameraController.transform.position.y;
        camPos[2] = cameraController.transform.position.z;

        logActive = new bool[6];
        for (int i = 0; i < audioLogs.Length; i++)
        {
            f = audioLogs[i].GetComponent<TextLog>().LogNum;
            logActive[f] = audioLogs[i].GetComponent<TextLog>().Active;
        }

        itemActive = new bool[6];
        for (int x = 0; x < items.Length; x++)
        {
            g = items[x].itemNum;
            itemActive[g] = items[x].Active;
        }

        MMRoomColor = new bool[19];
        for (int y = 0; y < rooms.Length; y++)
        {
            r = rooms[y].roomNum;
            MMRoomColor[r] = rooms[y].through;
        }

        MMPos = new float[2];
        MMPos[0] = minimap.transform.position.x;
        MMPos[1] = minimap.transform.position.y;
    }

}
