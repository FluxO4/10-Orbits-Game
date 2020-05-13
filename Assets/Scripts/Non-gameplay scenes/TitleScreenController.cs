using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject ContinueButton;
    public GameObject BeginReflection;

    int levelReached;
    public Slider backgroundSettings;
    public SceneController sceneController;
    public Material defaultSkybox;
    public Material pitchBlackSkybox;
    public Material classicSkybox;
    private void Start()
    {
        setBackground();
        levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        if(levelReached == 1)
        {
            startButton.SetActive(true);
            BeginReflection.SetActive(true);
        }
        else
        {
            ContinueButton.SetActive(true);
        }
    }

    public void updateBackgroundSettings()
    {
        PlayerPrefs.SetInt("Background", Mathf.RoundToInt(backgroundSettings.value));
        PlayerPrefs.Save();
        if (backgroundSettings.value == 0)
        {
            RenderSettings.skybox = pitchBlackSkybox;
        }else
        if(backgroundSettings.value == 1)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
        if (backgroundSettings.value == 2)
        {
            RenderSettings.skybox = classicSkybox;
        }
    }

    void setBackground()
    {
        int a = PlayerPrefs.GetInt("Background", 1);
        backgroundSettings.value = a;
        if (a == 0)
        {
            RenderSettings.skybox = pitchBlackSkybox;
        }
        else
       if (a == 1)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
       if (a == 2)
        {
            RenderSettings.skybox = classicSkybox;
        }
    }


    public void resetAll()
    {
        PlayerPrefs.DeleteAll();
        sceneController.reloadScene();
    }
}
