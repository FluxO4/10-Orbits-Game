using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Game
{
    //Controllers
    public static class Controller
    {
        public static SpeedController speedController;
        public static cameraMovement cameraMovementr;
        public static Settings settings;
        public static SceneController sceneController;
        public static MainController mainController;
    }

    //Prefabs
    public static class SceneObjects
    {


        public static GameObject satelliteP;

        public static GameObject selectedSat;

        public static List<GameObject> satellites = new List<GameObject>();

        public static List<GameObject> targets = new List<GameObject>();

        public static GameObject launchRing;

        public static GameObject launchArrow;

        public static Camera MainCamera;
    }

    public static class Prefabs
    {
        public static GameObject explosion;
        public static GameObject levelSetPrefab;
    }

    //UI Components
    public static class UI
    {
        public static Slider speedSlider;
        public static GameObject speedcontrollerUI;
        public static GameObject winBox;
        public static GameObject loseBox;
        public static Slider completionIndicator;
        public static Slider speedChangeStrengthSlider;
        public static GameObject quickRestartButton;
        public static Toggle quickRestartButtonToggle;
        public static GameObject launchButton;
        public static GameObject toggleTrailsButton;
        public static GameObject toggleTargetButton;
        public static GameObject infoText;
        public static Text levelNumber;
        public static Text levelName;
        public static GameObject doneButton;
        public static GameObject fuelUI;
        public static Slider fuelSlider;
        public static List<RectTransform> fuelIndicators;
        public static List<GameObject> winStars;
    }

    public static class Constants
    {
        public static float targetTimeSec;
        public static float gravityConstant;
        public static float cameraZoomFactor;
        public static float cameraMoveFactor;
        public static float speedChangeFactor;
        public static float minSatSpeed;
        public static float maxSatSpeed;
        public static float satLaunchFactor;
    }

    public static class toggles
    {
        public static bool selectingLaunchAngle = false;
        public static List<bool> satInTarget = new List<bool>();
    }

    public static List<Transform> Planets = new List<Transform>();
    public static List<float> PlanetMasses = new List<float>();

    public static void clearAll()
    {
        SceneObjects.satellites.Clear();
        SceneObjects.targets.Clear();
        Planets.Clear();
        PlanetMasses.Clear();
        toggles.satInTarget.Clear();
    }

    //STARS


}
