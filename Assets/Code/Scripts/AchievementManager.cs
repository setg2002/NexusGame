using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public enum Achievements { AllLogs, ImpossibleJump, GoodEnd, BadEnd, AllEndings, AllDoors };

    public GameObject achievementPopup;
    public GameObject achievementImg;
    public GameObject achievementTxt;
    // true when achievement is showing
    private bool achievmentFade;

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

        achievmentFade = false;

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

        if(achievmentFade == true)
        {
            achievementPopup.GetComponent<Image>().color = new Vector4(255, 255, 255, Mathf.Lerp(0, 255, 2 * Time.deltaTime));
            achievementTxt.GetComponent<Text>().color = new Vector4(255, 255, 255, Mathf.Lerp(0, 255, 2 * Time.deltaTime));
            achievementImg.GetComponent<Image>().color = new Vector4(255, 255, 255, Mathf.Lerp(0, 255, 2 * Time.deltaTime));
        }
        else
        {
            achievementPopup.GetComponent<Image>().color = new Vector4(255, 255, 255, Mathf.Lerp(achievementPopup.GetComponent<Image>().color.a, 0, 2 * Time.deltaTime));
            achievementTxt.GetComponent<Text>().color = new Vector4(255, 255, 255, Mathf.Lerp(achievementTxt.GetComponent<Text>().color.a, 0, 2 * Time.deltaTime));
            achievementImg.GetComponent<Image>().color = new Vector4(255, 255, 255, Mathf.Lerp(achievementImg.GetComponent<Image>().color.a, 0, 2 * Time.deltaTime));
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
                    StartCoroutine(AchivementPopup(achievementUI.GetComponent<Achievement>().Unlocked, achievementUI.transform.Find("AchievementText (1)").GetComponent<Text>().text));
                }
            }
        }
    }

    IEnumerator AchivementPopup(Sprite sprite, string text)
    {
        achievementImg.GetComponent<Image>().sprite = sprite;
        achievementTxt.GetComponent<Text>().text = text.ToString();
        achievmentFade = true;
        yield return new WaitForSeconds(4);
        achievmentFade = false;
    }
}
