/* NAME: AlienEncrypt.cs
 * PROGRAMMER: Alex Sandblom
 * PURPOSE: Resource File for Translator.cs
 * DATE: 1/25/19 6:08 p.m.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienEncrypt : MonoBehaviour
{
    // VARIABLES
    string encrypted;
    public int offset = 1;

    public string Encrypt(string plaintext)
    {
        encrypted = "";
        // letter shift encryption
        // Encrypt
        foreach (char p in plaintext)
        {
            // If the character is not a space, encrypt as usual
            if(p != ' ')
                encrypted += ((char)(p + offset)).ToString();
        }
        encrypted += ' ';
        return encrypted;
    }

    public string Decrypt(string encrypted)
    {
        string decrypted = "";
        // Decrypt
        foreach (char p in encrypted)
        {
            decrypted += ((char)(p - offset)).ToString();
        }
        return decrypted;
    }
}
