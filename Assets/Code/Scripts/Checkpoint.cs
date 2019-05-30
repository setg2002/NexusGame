using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SaveSystem.SavePlayer(
            GameObject.Find("Player1").GetComponent<PlayerCharacterController>(),
            GameObject.Find("Main_Camera").GetComponent<MetroidCameraController>(),
            GameObject.FindGameObjectsWithTag("TextLog"),
            GameObject.Find("Items").GetComponentsInChildren<ItemSave>(),
            GameObject.Find("Boundaries").transform.GetComponentsInChildren<Boundary>(),
            GameObject.Find("MinimapCamera")
            );

            SaveSystem.SaveAchievements(GameObject.Find("Player1").GetComponent<AchievementManager>());
        }
        else
        {
            return;
        }
    }
}
