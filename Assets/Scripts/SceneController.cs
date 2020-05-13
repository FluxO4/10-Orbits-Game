using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public GameObject loadingScreen;
    public void begin()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        SceneManager.LoadScene((PlayerPrefs.GetInt("LevelReached", 1)).ToString());
    }

    public void loadMainScene()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        SceneManager.LoadScene("Title");
    }

    public void loadLevelsScene()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        SceneManager.LoadScene("Levels");
    }

    public void loadCreditsScene()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        SceneManager.LoadScene("Credits");
    }

    public void loadLevel(string levelname)
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelname);
    }

    public void loadNextLevel()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.Save();
        string a = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene((int.Parse(a)+1).ToString());
    }

    public void reloadScene()
    {
        loadingScreen.SetActive(true);
        Time.timeScale = 1;
        Game.Controller.cameraMovementr = null;
        PlayerPrefs.Save();
        string a = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(a);
    }
}
