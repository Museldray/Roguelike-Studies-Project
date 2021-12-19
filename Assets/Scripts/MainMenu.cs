using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    public GameObject mainMenuPanel, optionsMenuPanel, deletePanel, instructionsPanel;

    public AudioSource mainMenuMusic;
    public Slider musicVolumeSlider;
    public Toggle fullScreen;
    public float startVolume;

    public CharacterSelector[] charactersToDelete;

    public bool fullScreenOn;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            startVolume = PlayerPrefs.GetFloat("volume", 1f);
        } else
        {
            startVolume = 0.35f;
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            if(PlayerPrefs.GetString("FullScreen") == "true")
            {
                fullScreen.isOn = true;
            } else
            {
                fullScreen.isOn = false;
            }

            Screen.SetResolution(Screen.width, Screen.height, fullScreen.isOn);
        } else
        {
            fullScreen.isOn = true;
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = startVolume;
        mainMenuMusic.volume = startVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if(musicVolumeSlider.value != mainMenuMusic.volume)
        {
            mainMenuMusic.volume = musicVolumeSlider.value;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuMusic.volume = startVolume;
        musicVolumeSlider.value = startVolume;
        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
    }

    public void SaveOptions()
    {
        if(musicVolumeSlider.value != startVolume)
        {
            SetGameVolume(musicVolumeSlider.value);
        }

        if (fullScreen.isOn)
        {
            Screen.SetResolution(Screen.width, Screen.height, true);
            PlayerPrefs.SetString("FullScreen", "true");
        } else
        {
            Screen.SetResolution(Screen.width, Screen.height, false);
            PlayerPrefs.SetString("FullScreen", "false");
        }

        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
    }

    public void SetGameVolume(float volume)
    {
        mainMenuMusic.volume = volume;
        startVolume = volume;

        PlayerPrefs.SetFloat("volume", volume);
    }

    public void ChangeGameResolution(string resolutionButtonName)
    {
        if(resolutionButtonName == "1920x1080")
        {
            Screen.SetResolution(1920, 1080, fullScreen.isOn);
        } 
        else if (resolutionButtonName == "1600x900")
        {
            Screen.SetResolution(1600, 900, fullScreen.isOn);
        } 
        else if (resolutionButtonName == "1280x720")
        {
            Screen.SetResolution(1280, 720, fullScreen.isOn);
        } 
        else if (resolutionButtonName == "960x540")
        {
            Screen.SetResolution(960, 540, fullScreen.isOn);
        }

    }

    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(false);

        foreach(CharacterSelector character in charactersToDelete)
        {
            PlayerPrefs.SetInt(character.playerToSpawn.name, 0);
        }
    }

    public void OpenInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
