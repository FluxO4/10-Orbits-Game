using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIlist : MonoBehaviour
{
    public Slider speedSlider;
    public GameObject speedcontrollerUI;
    public GameObject winBox;
    public GameObject loseBox;
    public Slider completionIndicator;
    public Slider speedChangeStrengthSlider;
    public GameObject quickRestartButton;
    public Toggle quickRestartButtonToggle;
    public GameObject launchButton;
    public GameObject toggleTrailsButton;
    public GameObject toggleTargetButton;
    public GameObject infoText;
    public Text levelNumber;
    public Text levelName;
    public GameObject doneButton;
    public GameObject fuelUI;
    public Slider fuelSlider;
    public List<RectTransform> fuelIndicators;
    public List<GameObject> winStars;

    void Awake()
    {
        Game.UI.speedcontrollerUI = speedcontrollerUI;
        Game.UI.speedSlider = speedSlider;
        Game.UI.winBox = winBox;
        Game.UI.loseBox = loseBox;
        Game.UI.completionIndicator = completionIndicator;
        Game.UI.speedChangeStrengthSlider = speedChangeStrengthSlider;
        Game.UI.quickRestartButton = quickRestartButton;
        Game.UI.quickRestartButtonToggle = quickRestartButtonToggle;
        Game.UI.launchButton = launchButton;
        Game.UI.toggleTargetButton = toggleTargetButton;
        Game.UI.toggleTrailsButton = toggleTrailsButton;
        Game.UI.infoText = infoText;
        Game.UI.levelName = levelName;
        Game.UI.levelNumber = levelNumber;
        Game.UI.doneButton = doneButton;
        Game.UI.fuelUI = fuelUI;
        Game.UI.fuelSlider = fuelSlider;
        Game.UI.fuelIndicators = fuelIndicators;
        Game.UI.winStars = winStars;
}

    private void Start()
    {
        //Player-set-settings
        

        {
            int t = PlayerPrefs.GetInt("QuickRestartButton", 1);
            if (t == 0)
            {
                Game.UI.quickRestartButton.SetActive(false);
                Game.UI.quickRestartButtonToggle.isOn = false;
            }
            else
            {
                //Game.UI.quickRestartButton.SetActive(true);
                Game.UI.quickRestartButtonToggle.isOn = true;
            }

        }
    }
}
