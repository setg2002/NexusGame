/* NAME: Translator.cs
 * PROGRAMMER: Alex Sandblom
 * PURPOSE: A script that gives the player a translator device to decrypt the aliens words over time
 * DATE: 1/25/19 8:13 p.m.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    // VARIABLES
    public Text text;
    //public List<string> words = new List<string>();
    public string result = "";
    public float delay = 0.1f;

    private bool CanShowText = true;

    string currentText;
    int offset;

    void Start()
    {
        text = GetComponent<Text>();
    }
    
    // Function to split the input line (example "Hello t") into two elements
    // the first element is the word in question and the second determines if it
    // gets encrypted or not.
    public void SplitAndEncrypt(string[] words)
    {
        result = null;
        foreach (string word in words)
        {
            string n = word.Substring(0, word.Length - 2);
            // add the plaintext word with a newline
            result += n + "\n";
        }
        
        StartCoroutine(ShowText());
    }

    

    IEnumerator ShowText()
    {
        text.text = "";
        if (CanShowText)
        {
            CanShowText = false;
            for (int i = 0; i <= result.Length; i++)
            {
                currentText = result.Substring(0, i);
                text.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(2);
            CanShowText = true;
        }
    }
   
}