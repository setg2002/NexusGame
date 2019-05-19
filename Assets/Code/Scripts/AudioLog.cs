using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog : MonoBehaviour
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
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(gameObject.activeSelf == true)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
    }


    IEnumerator ComAppear()
    {
        if (text.Length > 0)
        {
            ComBox.SetActive(true);   
            yield return new WaitForSeconds(3);
            Textbox.GetComponent<Translator>().SplitAndEncrypt(text);
            yield return new WaitForSeconds(15);
            ComBox.SetActive(false);
        }
    }
}
