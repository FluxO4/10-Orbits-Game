using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void toggleQuickRestartButton()
    {
        bool a = Game.UI.quickRestartButtonToggle.isOn;
        Game.UI.quickRestartButton.SetActive(a);
        if (a)
        {
            PlayerPrefs.SetInt("QuickRestartButton", 1);
        }
        else
        {
            PlayerPrefs.SetInt("QuickRestartButton", 0);
        }
        PlayerPrefs.Save();
    }
}
