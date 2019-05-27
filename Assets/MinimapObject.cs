using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapObject : MonoBehaviour
{
    // True when the map is fullscreen
    private bool MapLarge;

    public float toX;
    public float toY;

    private float toScale;



    void Awake()
    {
        MapLarge = false;
        toScale = 1;
        toX = 285;
        toY = 160;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (MapLarge == true)
            {
                toX = 285;
                toY = 160;
                toScale = 1;
                MapLarge = false;
            }
            else if (MapLarge == false)
            {
                toX = 0;
                toY = 0;
                toScale = 3;
                MapLarge = true;
            }
        }

        GetComponent<RectTransform>().localPosition = new Vector2(Mathf.Lerp(gameObject.transform.localPosition.x, toX, 2 * Time.deltaTime), Mathf.Lerp(gameObject.transform.localPosition.y, toY, 2 * Time.deltaTime));
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, toScale, 2 * Time.deltaTime), Mathf.Lerp(transform.localScale.y, toScale, 2 * Time.deltaTime), 1);
    }
}
