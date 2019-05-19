using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    public AudioMixer volumeControl;
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;
    public GameObject VolumeSlider;
    public GameObject ResDropdown;
    public GameObject VolumeEditor;
    public GameObject VolumeValueText;
    public GameObject fullscreenToggleBox;
    public GameObject Achievements_Panel;
    public GameObject SaveSelectPanel;

    Resolution[] resolutions;

    private float Vvalue = 80f;
    private float EValue;
    private float UIvalue;

    public void Start()
    {
        resolutions = Screen.resolutions;

        ResDropdown.GetComponent<Dropdown>().ClearOptions();

        List<string> ResOptions = new List<string>();

        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            ResOptions.Add(option);
        }

        ResDropdown.GetComponent<Dropdown>().AddOptions(ResOptions);
        ResDropdown.GetComponent<Dropdown>().value = currentResolutionIndex;
        ResDropdown.GetComponent<Dropdown>().RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSelectPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            Achievements_Panel.SetActive(false);
            MainMenuPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        if(File.Exists(Application.persistentDataPath + "/savedata.dat"))
        {
            MainMenuPanel.SetActive(false);
            SaveSelectPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("TestingScene");
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene("TestingScene");
    }

    public void NewSave()
    {
        File.Delete(Application.persistentDataPath + "/savedata.dat");
        SceneManager.LoadScene("TestingScene");
    }

    public void ResetAchievements()
    {
        File.Delete(Application.persistentDataPath + "/achievementdata.dat");
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitToDesktop()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void Achievements()
    {
        Achievements_Panel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void ExitSettings()
    {
        MainMenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        Vvalue = VolumeSlider.GetComponent<Slider>().value + 80;
        Vvalue *= 10;
        Vvalue = (int)Vvalue;
        Vvalue /= 10;

        volumeControl.SetFloat("MasterVolume", volume);

        VolumeEditor.GetComponentInChildren<Text>().text = "" + Vvalue;


        if (Vvalue % 10 == 0)
        {
            VolumeValueText.GetComponent<Text>().text = "" + Vvalue + ".0";
        }
        else
        {
            VolumeValueText.GetComponent<Text>().text = "" + Vvalue;
        }
    }

    public void EditVolume(string volume)
    {
        EValue = float.Parse(volume);


        EValue -= 80;
        VolumeSlider.GetComponent<Slider>().value = EValue;
        SetVolume(EValue);

    }

    public void UpdateUI()
    {
        volumeControl.GetFloat("MasterVolume", out UIvalue);
        VolumeSlider.GetComponent<Slider>().value = UIvalue;


        if (Screen.fullScreen)
        {
            fullscreenToggleBox.GetComponent<Toggle>().isOn = true;
        } else
        {
            fullscreenToggleBox.GetComponent<Toggle>().isOn = false;
        }
    }
}
