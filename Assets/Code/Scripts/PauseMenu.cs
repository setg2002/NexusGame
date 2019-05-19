using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public AudioMixer volumeControl;

    public GameObject PauseMenuPanel;
    public GameObject SettingsMenuPanel;
    public GameObject Achievements_Panel;
    public GameObject MinimapObject;
    public GameObject VolumeValueText;
    public GameObject VolumeSlider;
    public GameObject ResDropdown;
    public GameObject VolumeEditor;
    public GameObject fullscreenToggleBox;
    
    Resolution[] resolutions;

    private float Vvalue = 80f;
    private float EValue;
    public bool GamePaused = false;
    private float UIvalue;

    public SpriteRenderer[] rooms;

    private bool reactivateAudio = false;
    private bool reactivateItem = false;
    private bool reactivateUse = false;



    void Start()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        if (PauseMenuPanel.activeSelf == true)
        {
            PauseMenuPanel.SetActive(false);
            Debug.Log("Disabled pause menu in editor");
        }

        if(SettingsMenuPanel.activeSelf == true)
        {
            SettingsMenuPanel.SetActive(false);
            Debug.Log("Disabled settings menu in editor");
        }

        resolutions = Screen.resolutions;

        ResDropdown.GetComponent<Dropdown>().ClearOptions();

        List<string> ResOptions = new List<string>();


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


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))  
        {
            if (Achievements_Panel.activeSelf == true)
            {
                Achievements_Panel.SetActive(false);
                Pause();
            }
            else if (!GamePaused)
            {
                Pause();
            }
            else
            {
                if (SettingsMenuPanel.activeSelf == false)
                {
                    Resume();
                } else
                {
                    ExitSettings();
                }
            }
        }
    }

    public void Achievements()
    {
        Achievements_Panel.SetActive(true);
        PauseMenuPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        volumeControl.GetFloat("MasterVolume", out UIvalue);
        VolumeSlider.GetComponent<Slider>().value = UIvalue;

        if (Screen.fullScreen)
        {
            fullscreenToggleBox.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            fullscreenToggleBox.GetComponent<Toggle>().isOn = false;
        }
    }

    public void Pause()
    {
        GamePaused = true;
        Time.timeScale = 0f;
        PauseMenuPanel.SetActive(true);
        MinimapObject.SetActive(false);
        
        if(GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.activeSelf)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(false);
            reactivateAudio = true;
        }
        if(GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.activeSelf)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.SetActive(false);
            reactivateUse = true;
        }
        if(GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.activeSelf)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.SetActive(false);
            reactivateItem = true;
        }
    }

    public void Resume()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        PauseMenuPanel.SetActive(false);
        MinimapObject.SetActive(true);

        if (reactivateAudio)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(true);
            reactivateAudio = false;
        }
        if (reactivateItem)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().itemsText.gameObject.SetActive(true);
            reactivateItem = false;
        }
        if (reactivateUse)
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().useText.gameObject.SetActive(true);
            reactivateUse = false;
        }
    }

    public void OpenSettings()
    {
        PauseMenuPanel.SetActive(false);
        SettingsMenuPanel.SetActive(true);
    }

    public void ExitSettings()
    {
        PauseMenuPanel.SetActive(true);
        SettingsMenuPanel.SetActive(false);
    }

    public void QuitToMenu()
    {
        SaveSystem.SavePlayer(
            GameObject.Find("Player1").GetComponent<PlayerCharacterController>(),
            GameObject.Find("Main_Camera").GetComponent<MetroidCameraController>(),
            GameObject.FindGameObjectsWithTag("TextLog"),
            GameObject.Find("Items").GetComponentsInChildren<ItemSave>(),
            GameObject.Find("Boundaries").transform.GetComponentsInChildren<Boundary>(),
            GameObject.Find("MinimapCamera")
            );

        SaveSystem.SaveAchievements(GameObject.Find("Player1").GetComponent<AchievementManager>());

        SceneManager.LoadScene("MainMenu");
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

    public void EditVolume(string volume)
    {
        EValue = float.Parse(volume);


        EValue -= 80;
        VolumeSlider.GetComponent<Slider>().value = EValue;
        SetVolume(EValue);

    }

    public void SetVolume(float volume)
    {
        Vvalue = VolumeSlider.GetComponent<Slider>().value + 80;
        Vvalue *= 10;
        Vvalue = (int)Vvalue;
        Vvalue /= 10;

        volumeControl.SetFloat("MasterVolume", volume);

        VolumeEditor.GetComponentInChildren<Text>().text = "" +Vvalue;


        if (Vvalue % 10 == 0)
        {
            VolumeValueText.GetComponent<Text>().text = "" + Vvalue + ".0";
        }
        else
        {
            VolumeValueText.GetComponent<Text>().text = "" +Vvalue;
        }
    }
}
