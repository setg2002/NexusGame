using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public enum Achievements { AllLogs, ImpossibleJump, GoodEnd, BadEnd, AllEndings, AllDoors };

    public List<int> CompletedAchievements;

    public List<int> Doors;

    public GameObject[] AchievementUI;

    // The number of audio logs in the level for all logs collected achievement
    private int logsInScene;
    private int doorsInScene;


    private void Awake()
    {
        AchievementData data = SaveSystem.LoadAchievements();

        if(data == null)
        {
            CompletedAchievements = new List<int>();
        }
        else
        {
            CompletedAchievements = data.CompletedAchievements;
        }

        logsInScene = GameObject.FindGameObjectsWithTag("TextLog").Length;

        doorsInScene = GameObject.Find("TheAndromeda").transform.Find("Doors").transform.childCount;

        //Debug.Log();
    }

    void Update()
    {
        if(GetComponent<PlayerCharacterController>().logCount == logsInScene)
        {
            AchievementGet(Achievements.AllLogs);
        }

        if(Doors.ToArray().Length == doorsInScene)
        {
            AchievementGet(Achievements.AllDoors);
        }

    }

    public void AchievementGet(Achievements achievement)
    {
        if(!CompletedAchievements.Contains((int)achievement))
        {
            Debug.Log("Achievement: " + achievement);

            CompletedAchievements.Add((int)achievement);

            foreach(GameObject achievementUI in AchievementUI)
            {
                if(achievementUI.GetComponent<Achievement>().thisAchievement == achievement)
                {
                    achievementUI.GetComponent<Achievement>().locked = false;
                }
            }
        }
    }
}
