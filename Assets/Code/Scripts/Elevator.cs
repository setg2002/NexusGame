using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject connectedElevator;

    private GameObject Walls;
    private GameObject Doors;





    private void OnDrawGizmos()
    {
        if (connectedElevator != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(connectedElevator.transform.position.x - 0.5f, connectedElevator.transform.position.y - 0.5f));
            connectedElevator.GetComponent<Elevator>().connectedElevator = gameObject;
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector3(4, 4, 0));
        }
    }

    private void Awake()
    {
        Walls = gameObject.transform.Find("BG").Find("Walls").gameObject;
        Doors = gameObject.transform.Find("BG").Find("Doors").gameObject;

        if (connectedElevator)
        {
            connectedElevator.GetComponent<Elevator>().connectedElevator = gameObject;
        }
    }

    public void UseElevator(GameObject player, GameObject camera)
    {
        StartCoroutine(E(player, camera));
    }

    private IEnumerator E (GameObject player, GameObject camera)
    {
        foreach(SpriteRenderer door in Doors.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            door.sortingOrder = 5;
        }
        foreach(SpriteRenderer door in connectedElevator.GetComponent<Elevator>().Doors.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            door.sortingOrder = 5;
        }

        gameObject.GetComponent<Animator>().Play("DoorsClose", -1, 0);
        connectedElevator.GetComponent<Animator>().Play("DoorsClose", -1, 0);

        Walls.SetActive(true);
        connectedElevator.GetComponent<Elevator>().Walls.SetActive(true);

        yield return new WaitForSeconds(2);
        player.transform.position = connectedElevator.transform.position;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, camera.transform.position.z);
        gameObject.GetComponent<Animator>().Play("DoorsOpen", -1, 0);
        connectedElevator.GetComponent<Animator>().Play("DoorsOpen", -1, 0);

        yield return new WaitForSeconds(0.6f);
        Walls.SetActive(false);
        connectedElevator.GetComponent<Elevator>().Walls.SetActive(false);

        foreach (SpriteRenderer door in Doors.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            door.sortingOrder = 0;
        }
        foreach (SpriteRenderer door in connectedElevator.GetComponent<Elevator>().Doors.transform.GetComponentsInChildren<SpriteRenderer>())
        {
            door.sortingOrder = 0;
        }
    }
}
