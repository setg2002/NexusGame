using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static GameObject currentPoint;

    public static void SetCurrentCheckpoint(GameObject point)
    {
        if(currentPoint && currentPoint != point)
            currentPoint.GetComponent<Checkpoint>().claimed = false;
        currentPoint = point;
    }
    public static void ResetLevel()
    {
        if(currentPoint)
            GameObject.Find("Player").transform.position = currentPoint.transform.position;
    }
}
