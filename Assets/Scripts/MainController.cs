using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{

    bool levelWon = false;
    bool levelLost = false;

    public string levelName;
    [HideInInspector]
    public int satellitesDone = 0;

    public float fuelAmount;

    
    public List<float> fuelCheckPoints;

    public bool teleportCameraToNextSatellite = false;

    int numSat;
    float timeInTarget = 0;

    void setLevelName()
    {
        Game.UI.levelNumber.text = "LEVEL " + SceneManager.GetActiveScene().name;
        Game.UI.levelName.text = levelName;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ToggleTarget(bool yes)
    {
        foreach (GameObject t in Game.SceneObjects.targets)
        {
            if (yes)
            {
                t.SetActive(true);
            }
            else
            {
                t.SetActive(false);
            }
        }
    }

    public void ToggleTrails(bool yes)
    {
        foreach (GameObject s in Game.SceneObjects.satellites)
        {
            TrailRenderer r = s.GetComponent<TrailRenderer>();
            if (!yes)
            {
                r.enabled = false;
            }
            else
            {
                r.Clear();
                r.enabled = true;
            }
        }
    }



    float fuelValue;
    public void winGame()
    {
        if (!levelWon)
        {
            levelWon = true;
            string levelname = SceneManager.GetActiveScene().name;
            int levelnumber = int.Parse(levelname);

            PlayerPrefs.SetInt("LevelReached", levelnumber + 1);
            
            fuelValue = Game.UI.fuelSlider.value;
            int score = 0;
            for(int i = 0; i < fuelCheckPoints.Count; i++)
            {
                if(fuelValue >= fuelCheckPoints[i])
                {
                    Game.UI.winStars[i].SetActive(true);
                    score += 1;
                }
            }
            
            if (PlayerPrefs.GetInt("Level"+levelname+"Score", -1) < score)
            {
               // PlayerPrefs.SetInt("TotalStars", PlayerPrefs.GetInt("TotalStars", 0) + score - PlayerPrefs.GetInt("Level" + levelname + "Score", -1));
                int temp = PlayerPrefs.GetInt("StarsCollected", 0);
                PlayerPrefs.SetInt("StarsCollected", temp + score - PlayerPrefs.GetInt("Level" + levelname + "Score", 0));
                PlayerPrefs.SetInt("Level" + levelname + "Score", score);
                PlayerPrefs.Save();
            }

            
        }
    }

    public void loseGame()
    {
        if (!levelWon)
        {
            levelLost = true;
            if (!Game.UI.loseBox.activeSelf)
            {
                Game.UI.loseBox.SetActive(true);
            }
            if (Game.UI.winBox.activeSelf)
            {
                Game.UI.winBox.SetActive(false);
            }
        }
    }

    private void Start()
    {
        setLevelName();
        numSat = Game.toggles.satInTarget.Count;
        string levelname = SceneManager.GetActiveScene().name;
        int levelnumber = int.Parse(levelname);
        PlayerPrefs.SetInt("LevelReached", levelnumber);
    }

    public void selectNextSat()
    {
        satellitesDone++;
        if (satellitesDone < Game.SceneObjects.satellites.Count)
        {
            {
                Game.SceneObjects.selectedSat = Game.SceneObjects.satellites[satellitesDone];
                if(teleportCameraToNextSatellite)
                Game.Controller.cameraMovementr.gameObject.transform.position = new Vector3(Game.SceneObjects.selectedSat.transform.position.x, Game.Controller.cameraMovementr.gameObject.transform.position.y, Game.SceneObjects.selectedSat.transform.position.z);
            }
            Game.UI.speedcontrollerUI.SetActive(false);
            Game.UI.doneButton.SetActive(false);
            Game.UI.launchButton.SetActive(true);
            Game.SceneObjects.launchRing.SetActive(true);
        }
    }
    private void Update()
    {
       // Debug.Log(timeInTarget);
        {
            bool allSatsInTarget = true;
            for (int i = 0; i < numSat; i++)
            {
               // Debug.Log("SAT " + i + "IS IN TARGET? " + Game.toggles.satInTarget[i] + " " + Time.time);
                if (!Game.toggles.satInTarget[i])
                {
                    allSatsInTarget = false;
                }
            }
            if (allSatsInTarget)
            {
                timeInTarget += Time.deltaTime;
                Game.UI.completionIndicator.value = timeInTarget / Game.Constants.targetTimeSec;
                if (timeInTarget >= Game.Constants.targetTimeSec)
                {
                    winGame();
                }
            }
            else
            {
                timeInTarget = 0;
                Game.UI.completionIndicator.value = 0;
            }
        }

        if (levelLost)
        {
            if (!Game.UI.loseBox.activeSelf)
            {
                Game.UI.loseBox.SetActive(true);
            }
            if (Game.UI.winBox.activeSelf)
            {
                Game.UI.winBox.SetActive(false);
            }
        }
        else if (levelWon)
        {
            if (!Game.UI.winBox.activeSelf)
            {
                Game.UI.winBox.SetActive(true);
            }
        }
    }
}
