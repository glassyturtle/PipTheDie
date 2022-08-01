using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu, creditsMenu;
    [SerializeField] GameObject normalMenu;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Dropdown resolutionDropDown;
    [SerializeField] AudioSource AS;

    Resolution[] resolutions;




    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }


        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetRes(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    public void OpenOptionsMenu(bool optionsIsOpen)
    {
        AS.Play();
        if (optionsIsOpen)
        {
            optionsMenu.SetActive(false);
            normalMenu.SetActive(true);
        }
        else
        {
            optionsMenu.SetActive(true);
            normalMenu.SetActive(false);
        }

    }
    public void OpenCredits(bool optionsIsOpen)
    {
        AS.Play();
        if (optionsIsOpen)
        {
            creditsMenu.SetActive(false);
            normalMenu.SetActive(true);
        }
        else
        {
            creditsMenu.SetActive(true);
            normalMenu.SetActive(false);
        }

    }

    public void SetFullScreen(bool isFullScreen)
    {
        AS.Play();
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float amount)
    {
        mixer.SetFloat("Volume", amount);
    }

    public void StartGame()
    {
        AS.Play();
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        AS.Play();
        Application.Quit();
    }
}
