using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerCharacterController>().HasBlackBox == true)
            {
                File.Delete(Application.persistentDataPath + "/savedata.dat");
                SceneManager.LoadScene("Good Credits");
                collision.gameObject.GetComponent<AchievementManager>().AchievementGet(AchievementManager.Achievements.GoodEnd);
            }
            else
            {
                File.Delete(Application.persistentDataPath + "/savedata.dat");
                SceneManager.LoadScene("Bad Credits");
                collision.gameObject.GetComponent<AchievementManager>().AchievementGet(AchievementManager.Achievements.BadEnd);
            }

            // Achievement for if the player has completed both endings
            if(collision.gameObject.GetComponent<AchievementManager>().CompletedAchievements.Contains((int)AchievementManager.Achievements.BadEnd) && collision.gameObject.GetComponent<AchievementManager>().CompletedAchievements.Contains((int)AchievementManager.Achievements.GoodEnd))
            {
                collision.gameObject.GetComponent<AchievementManager>().AchievementGet(AchievementManager.Achievements.AllEndings);
            }

            SaveSystem.SaveAchievements(collision.gameObject.GetComponent<AchievementManager>());
        }
    }
}
