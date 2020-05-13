using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initialiser : MonoBehaviour
{
    public SpeedController speedController;
    public Settings settings;
    public cameraMovement cameraMovementr;
    public Camera mainCamera;
    public SceneController sceneController;
    public MainController mainController;

    Vector3 cameraStartPos;
    
    public Slider backgroundSettings;
    public Material defaultSkybox;
    public Material pitchBlackSkybox;
    public Material classicSkybox;
    void Awake()
    {
        Game.clearAll();
        Game.Controller.speedController = speedController;
        Game.Controller.settings = settings;
        Game.Controller.cameraMovementr = cameraMovementr;
        Game.SceneObjects.MainCamera = mainCamera;
        cameraStartPos = Game.SceneObjects.MainCamera.transform.position;
        Game.SceneObjects.MainCamera.transform.position = new Vector3(100,60,100);
        Game.Controller.sceneController = sceneController;
        Game.Controller.mainController = mainController;
        
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("TargetArea");
            foreach (GameObject target in targets)
            {
                Game.SceneObjects.targets.Add(target);
            }
        }
        {
            GameObject[]  satellites = GameObject.FindGameObjectsWithTag("Satellite");
            int numSat = satellites.Length;
            
            int[] indices = new int[numSat];
            {
                for(int i = 0; i < numSat; i++)
                {
                    SatelliteProperties temp = satellites[i].GetComponent<SatelliteProperties>();
                    indices[i] = temp.launchOrder;
                }
            }
            {
                for (int i = numSat-1; i >= 0; i--)
                {
                    for (int ii = 0; ii < i; ii++)
                    {
                        if(indices[ii] > indices[ii+1])
                        {
                            int itemp = indices[ii];
                            indices[ii] = indices[ii + 1];
                            indices[ii + 1] = itemp;
                            GameObject temp = satellites[ii];
                            satellites[ii] = satellites[ii+1];
                            satellites[ii + 1] = temp;
                        }
                    }
                }
            }


            for (int i = 0; i < numSat; i++)
            {

                Game.SceneObjects.satellites.Add(satellites[i]);
                Game.toggles.satInTarget.Add(false);
                Game.SceneObjects.satellites[i].name = i.ToString();
            }
            Game.SceneObjects.selectedSat = Game.SceneObjects.satellites[0];
        }

        
        {
            GameObject[] Planets = GameObject.FindGameObjectsWithTag("Planet");

            foreach (GameObject Planet in Planets)
            {
                
                Game.Planets.Add(Planet.transform);
                planetProperties temp = Planet.transform.GetComponent<planetProperties>();
                Game.PlanetMasses.Add(temp.mass);
            }
        }

      
        setBackground();
    }

    public void updateBackgroundSettings()
    {
        PlayerPrefs.SetInt("Background", Mathf.RoundToInt(backgroundSettings.value));
        PlayerPrefs.Save();
        setBackground();
    }

    void setBackground()
    {
        int a = PlayerPrefs.GetInt("Background", 1);
        backgroundSettings.value = a;
        


        if (a == 0)
        {
            RenderSettings.skybox = pitchBlackSkybox;

            foreach (GameObject target in Game.SceneObjects.targets)
            {
                MeshRenderer targetMat = target.GetComponent<MeshRenderer>();
                targetMat.material.color = new Color(0.1f,0.1f,0.2f,0.9f);
            }
        }
        else
        if (a == 1)
        {
            RenderSettings.skybox = defaultSkybox;
            foreach (GameObject target in Game.SceneObjects.targets)
            {
                MeshRenderer targetMat = target.GetComponent<MeshRenderer>();
                targetMat.material.color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
        else
        if (a == 2)
        {
            RenderSettings.skybox = classicSkybox;
            foreach (GameObject target in Game.SceneObjects.targets)
            {
                MeshRenderer targetMat = target.GetComponent<MeshRenderer>();
                targetMat.material.color = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            }
        }
    }

    public void startGame()
    {
        Game.SceneObjects.MainCamera.transform.position = cameraStartPos;
        Game.UI.launchButton.SetActive(true);
        Game.UI.toggleTrailsButton.SetActive(true);
        Game.UI.toggleTargetButton.SetActive(true);
        Game.UI.infoText.SetActive(true);
        if (Game.Controller.mainController.fuelCheckPoints.Count >= 4)
        {
            Game.UI.fuelUI.SetActive(true);
        }

        {
            int t = PlayerPrefs.GetInt("QuickRestartButton", 1);
            if (t == 0)
            {
                Game.UI.quickRestartButton.SetActive(false);
                Game.UI.quickRestartButtonToggle.isOn = false;
            }
            else
            {
                Game.UI.quickRestartButton.SetActive(true);
                Game.UI.quickRestartButtonToggle.isOn = true;
            }

        }
    }

    private void Start()
    {
        if (Game.Controller.mainController.fuelCheckPoints.Count >= 4)
        {

            float m = Game.Controller.mainController.fuelAmount;

            Game.UI.fuelSlider.maxValue = m;
            Game.UI.fuelSlider.value = m;

            RectTransform t = Game.UI.fuelUI.transform.parent.GetComponent<RectTransform>();
            //Fuel slider height is 59 percent of (screen.height * 1280/screen.width)


            float h = 0.59f * t.sizeDelta.y;
            //float h = t.an;


            for (int i = 0; i < Game.Controller.mainController.fuelCheckPoints.Count; i++)
            {
                Game.UI.fuelIndicators[i].anchoredPosition = new Vector2(-14, h * Game.Controller.mainController.fuelCheckPoints[i] / (m * 1.0f));
            }
        }
        //Game.SceneObjects.selectedSat = Game.SceneObjects.satellites[0];
        {
            Game.SceneObjects.selectedSat = Game.SceneObjects.satellites[0];
            /*satellitePhysics temp = Game.SceneObjects.selectedSat.GetComponent<satellitePhysics>();
            temp.activeSatellite = true;*/
        }

    }
}
