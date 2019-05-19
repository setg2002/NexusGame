using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool claimed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            claimed = true;
            CheckpointManager.SetCurrentCheckpoint(gameObject);
        }
    }
}
