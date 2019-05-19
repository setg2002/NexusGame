using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AchievementData
{
    public List<int> CompletedAchievements;
    public List<int> Doors;


    public AchievementData(AchievementManager achievementManager)
    {
        CompletedAchievements = achievementManager.CompletedAchievements;
        Doors = achievementManager.Doors;
    }

}