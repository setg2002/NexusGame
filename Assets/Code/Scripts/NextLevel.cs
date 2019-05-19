using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public static float currentLevel = 1;
    public static float maxLevels;
    private void Start()
    {
        maxLevels = SceneManager.sceneCountInBuildSettings - 2;
    }
    public void nextLevel()
    {
        if (currentLevel + 1 <= maxLevels)
        {
            currentLevel++;
            SceneManager.LoadScene("Level " + currentLevel.ToString());
        }
        else
        {
            SceneManager.LoadScene("Credits");
            currentLevel = 0;
        }


    }
}
