using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text itemsText;
    public Text useText;
    public Text audioText;

    // Start is called before the first frame update
    void Start()
    {
        useText.gameObject.SetActive(false);
        audioText.gameObject.SetActive(false);
    }



    public void updateItem(string item)
    {
        if (item == null)
        {
            itemsText.text = "";
            return;
        }
        itemsText.text = item;
    }


    // If it requires an item
    public void displayRequiredItem(GameObject requiredItem, string currentItem)
    {
        if (currentItem != requiredItem.name)
        {
            useText.gameObject.SetActive(true);
            useText.text = "You need the required item to continue";
        }
        else
        {
            useText.gameObject.SetActive(true);
            useText.text = "Press E to use " + requiredItem.name;
        }
    }


    // If it does not require an item
    public void displayRequiredItem(string str)
    {
        useText.gameObject.SetActive(true);
        useText.text = "Press E to use" + str;
    }


    public void hideRequiredItem()
    {
        useText.gameObject.SetActive(false);
    }


    public void displayAudioLog()
    {
        audioText.gameObject.SetActive(true);
    }


    public void hideAudioLog()
    {
        audioText.gameObject.SetActive(false);
    }
}
