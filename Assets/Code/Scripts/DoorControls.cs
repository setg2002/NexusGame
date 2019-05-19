using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControls : MonoBehaviour
{
    // True when the door is open
    public bool doorOpen;

    float time;

    // The number ID of the current door
    public int doorNum;

    // The position the door should be when closed for lerp
    Vector3 ClosedPos;

    // Collider that adds collision to the door and is turned off when the door opens
    public BoxCollider2D OpenCollider;

    private EdgeCollider2D Block;

    // Audio variables
    public AudioSource source;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;



    void Awake()
    {
        Block = gameObject.GetComponent<EdgeCollider2D>();
        OpenCollider = gameObject.GetComponent<BoxCollider2D>();

        ClosedPos = transform.position;
    }

    // Constantly checks if the door is powered, and if so destroys the collider after 0.75 seconds
    private void Update()
    {

        if (gameObject.CompareTag("powered") || gameObject.CompareTag("Quarentine"))
        {
            time += Time.deltaTime;
        }
        if(doorOpen == true)
        {
            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(transform.position.y, ClosedPos.y + 3, 6 * Time.deltaTime), 0);
            OpenCollider.offset = new Vector2(0, Mathf.SmoothStep(OpenCollider.offset.y, -3.75f, 6 * Time.deltaTime));
        }
        if (doorOpen == false)
        {
            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(transform.position.y, ClosedPos.y, 6 * Time.deltaTime), 0);
            OpenCollider.offset = new Vector2(0, Mathf.SmoothStep(OpenCollider.offset.y, -0.75f, 6 * Time.deltaTime));
        }
    }

    // Checks for collisions with the player and opens the door when they enter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && gameObject.CompareTag("powered"))
        {
            source.PlayOneShot(DoorOpen, 0.5f);

            doorOpen = true;
        }

        if(!other.GetComponent<AchievementManager>().Doors.Contains(doorNum))
        {
            other.GetComponent<AchievementManager>().Doors.Add(doorNum);
        }
    }

    // Checks for collisions with the player and closes the door when they exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if(doorOpen)
        {
            source.PlayOneShot(DoorClose, 0.5f);

            doorOpen = false;
        }
    }
}
