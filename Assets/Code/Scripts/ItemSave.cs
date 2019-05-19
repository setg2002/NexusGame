using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSave : MonoBehaviour
{
    public int itemNum;

    public bool Active;




    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            Active = true;
        }
        else
        {
            Active = data.itemActive[itemNum];
        }

        if (Active == true)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameObject.activeSelf == true)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
    }
}
