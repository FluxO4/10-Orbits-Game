using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpeedController : MonoBehaviour
{
    public event Action speedZeroed;
    public event Action speedUpdated;

    bool holding;
    public void UpdateSpeed()
    {
        speedUpdated?.Invoke();
    }

    public void UpdateSpeedChangeStrengthSlider()
    {
        Game.Constants.speedChangeFactor = Game.UI.speedChangeStrengthSlider.value;

        //Debug.Log(Game.Controller.settings.speedChangeFactor);
        PlayerPrefs.SetFloat("SpeedChangeSensitivity", Game.Constants.speedChangeFactor);
        PlayerPrefs.Save();
    }

    public void hold(bool yes)
    {
        if (yes)
        {
            holding = true;
        }
        else
        {
            holding = false;
        }
    }

    public void activate(bool yes)
    {
        if (yes)
        {
            Game.UI.speedSlider.value = 0;
            Game.UI.speedcontrollerUI.SetActive(true);
        }
        else
        {
            Game.UI.speedSlider.value = 0;
            Game.UI.speedcontrollerUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Game.UI.speedcontrollerUI.activeSelf && !holding)
        {
            if (Game.UI.speedSlider.value != 0)
            {

                Game.UI.speedSlider.value *= 0.8f;
                if (Mathf.Abs(Game.UI.speedSlider.value) < 0.01f)
                {
                    Game.UI.speedSlider.value = 0;
                    speedZeroed?.Invoke();
                }
            }
            else
            {
                speedZeroed?.Invoke();
            }
        }
    }
}
