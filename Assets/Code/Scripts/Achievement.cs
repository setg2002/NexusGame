using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Achievement : AchievementManager
{
    public Achievements thisAchievement;
    public bool locked;

    public Sprite Lock;
    public Sprite Unlocked;



    private void Awake()
    {
        AchievementData data = SaveSystem.LoadAchievements();

        if (data == null)
        {
            locked = true;
        }
        else
        {
            if (data.CompletedAchievements.Contains((int)thisAchievement))
            {
                locked = false;
            }
            else
            {
                locked = true;
            }
        }
    }

    void Update()
    {
        if (locked == true)
        {
            GetComponentInChildren<Image>().sprite = Lock;
        }
        if (locked == false)
        {
            GetComponentInChildren<Image>().sprite = Unlocked;
        }
    }
}
