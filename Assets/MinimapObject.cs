using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapObject : MonoBehaviour
{
    // True when the map is fullscreen
    private bool MapLarge;

    private float toX;
    private float toY;

    private float toScale;



    void Awake()
    {
        MapLarge = false;
        toScale = 1;
        toX = 780;
        toY = 440;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapLarge == true)
            {
                toX = 780;
                toY = 440;
                toScale = 1;
                MapLarge = false;
            }
            else if (MapLarge == false)
            {
                toX = 470;
                toY = 275;
                toScale = 3;
                MapLarge = true;
            }
        }

        transform.position = new Vector2(Mathf.Lerp(gameObject.transform.position.x, toX, 2 * Time.deltaTime), Mathf.Lerp(gameObject.transform.position.y, toY, 2 * Time.deltaTime));
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, toScale, 2 * Time.deltaTime), Mathf.Lerp(transform.localScale.y, toScale, 2 * Time.deltaTime), 1);
    }
}
