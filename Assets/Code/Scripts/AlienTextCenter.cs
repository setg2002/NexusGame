using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienTextCenter : MonoBehaviour
{
    public string[] text;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Translated").GetComponent<Translator>().SplitAndEncrypt(text);
    }
}
