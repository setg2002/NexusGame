using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLog : MonoBehaviour
{
    public int LogNum;

    public bool Active;

    public string[] text;

    public GameObject Textbox;
    public GameObject ComBox;

    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            Active = true;
        }
        else
        {
            Active = data.logActive[LogNum];
        }

        if (Active == true)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public IEnumerator ComAppear()
    {
        if (text.Length > 0)
        {
            ComBox.SetActive(true);
            yield return new WaitForSeconds(1);
            Textbox.GetComponent<Translator>().SplitAndEncrypt(text);
            yield return new WaitForSeconds(23);
            ComBox.SetActive(false);
        }
    }


    private void Update()
    {
        if(GetComponent<SpriteRenderer>().enabled == true)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
    }


    
}
