using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public float targetTimeSec = 10;
    public float gravityConstant = 1;
    public float cameraZoomFactor = 1;
    public float cameraMoveFactor = 1;
    public float speedChangeFactor = 1;
    public float minSatSpeed = 0.1f;
    public float maxSatSpeed = 10;
    public float satLaunchFactor = 5;
    void Awake()
    {
        Game.Constants.targetTimeSec = targetTimeSec;
        Game.Constants.gravityConstant = gravityConstant;
        Game.Constants.cameraMoveFactor = cameraMoveFactor;
        Game.Constants.cameraZoomFactor = cameraZoomFactor;
        //Game.Constants.speedChangeFactor = speedChangeFactor;



        Game.Constants.minSatSpeed = minSatSpeed;
        Game.Constants.maxSatSpeed = maxSatSpeed;
        Game.Constants.satLaunchFactor = satLaunchFactor;
    }

    private void Start()
    {
        {
            float t = PlayerPrefs.GetFloat("SpeedChangeSensitivity", speedChangeFactor);
            Game.UI.speedChangeStrengthSlider.value = t;
            Game.Controller.settings.speedChangeFactor = t;
        }
    }
}
