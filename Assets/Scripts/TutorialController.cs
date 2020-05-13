using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    LaunchRing launchRingScript;

    [SerializeField]
    Flashing launchButtonFlashingScript;

    [SerializeField]
    GameObject clickLaunchButtonText;

    [SerializeField]
    GameObject launchCircleHighlights;

    [SerializeField]
    UnityEngine.UI.Text loseText;

    [SerializeField]
    GameObject lowerBox;

    [SerializeField]
    Vector2 targetMinMaxRadius;

    [SerializeField]
    GameObject speedChangeIndicator;

    [SerializeField]
    TextMesh speedChangeText;

    [SerializeField]
    GameObject speedUpImage;

    [SerializeField]
    GameObject slowDownImage;

    [SerializeField]
    GameObject theRocketScienceButton;

    [SerializeField]
    GameObject info1;

    [SerializeField]
    GameObject info2;


    public void activateRocketScienceButton()
    {
        theRocketScienceButton.SetActive(true);
    }


    float speedChangeIndicationTime = 3;
   

    bool setSpeedChangeIndicator = false;
    bool afterLaunch = false;

    Transform satellite;
    satellitePhysics satPhys;
    private void Start()
    {
        //Debug.Log("Initiated tutorial controller");
        launchRingScript.LaunchSat += Launch;
        launchRingScript.ClickInCircle += ClickInCircle;
        Game.Controller.speedController.speedUpdated += resetSCI;

        satellite = Game.SceneObjects.selectedSat.transform;
        satPhys = satellite.GetComponent<satellitePhysics>();

        satPhys.destroyed += OnSatelliteDestroy;
    }

    void OnSatelliteDestroy()
    {
        this.enabled = false;
    }

    IEnumerator AfterLaunchDelay()
    {
        yield return new WaitForSeconds(5);
        afterLaunch = true;
    }

    IEnumerator SpeedChangeIndicatorDelay()
    {
        yield return new WaitForSeconds(speedChangeIndicationTime);
        speedChangeIndicator.SetActive(false);
        setSpeedChangeIndicator = false;
    }

    void Launch()
    {
        info1.SetActive(false);
        info2.SetActive(true);
        StartCoroutine(AfterLaunchDelay());
        //Debug.Log("Launch strength: " + launchRingScript.launchVector.magnitude);
        if(launchRingScript.launchVector.magnitude > 3.0)
        {
            //Debug.Log("That was too fast! The satellite will be lost! Try launching slower!");
        }else
        if (launchRingScript.launchVector.magnitude < 1.8)
        {
            //Debug.Log("That was too fast! The satellite will be lost! Try launching slower!");
            loseText.text = "You launched the satellite too slow! Try launching it faster by making the arrow bigger!";
        }

        if (Vector3.Dot(-satellite.position, launchRingScript.launchVector)/(satellite.position.magnitude * launchRingScript.launchVector.magnitude) < -0.8)
        {
            //Debug.Log("Oops! You launched the satellite away from the planet! Try launching it more towards it!");
            loseText.text = "Oops! You launched the satellite away from the planet! Try launching it more towards it!";
        }
        else
        if (Vector3.Dot(-satellite.position, launchRingScript.launchVector) / (satellite.position.magnitude * launchRingScript.launchVector.magnitude) > 0.5)
        {
            //Debug.Log("Oops! You launched the satellite towards the planet! Try launching it a little away from it!");
            loseText.text = "Oops! You launched the satellite towards the planet! Try launching it a little away from it!";
        }

    }

    void resetSCI()
    {
        speedUpImage.SetActive(false);
        slowDownImage.SetActive(false);
        satPhys.resetOrbitData();
        speedChangeIndicator.SetActive(false);
        StopCoroutine(SpeedChangeIndicatorDelay());
        StartCoroutine(SpeedChangeIndicatorDelay());
    }

    void ClickInCircle()
    {
        //Debug.Log("REACHED CLICKINCIRCLE IN TUTORIAL CONTROLLER");
        launchCircleHighlights.SetActive(false);
        launchButtonFlashingScript.enabled = true;
        clickLaunchButtonText.SetActive(true);
    }
    private void Update()
    {
        if (afterLaunch) {


            if (satellite.position.magnitude > 10)
            {
                loseText.text = "The satellite went too far away! It will be very hard to get it back! Try launching it slower";
                Game.Controller.mainController.loseGame();
                this.enabled = false;
            }

            if (!setSpeedChangeIndicator)
            {

                if(satPhys.minPoint.magnitude < targetMinMaxRadius.x)
                {
                    setSpeedChangeIndicator = true;
                    speedChangeIndicator.SetActive(true);
                    speedChangeText.text = "Speed up here!";
                    speedUpImage.SetActive(true);
                    speedChangeIndicator.transform.position = satPhys.maxPoint;
                    //StartCoroutine(SpeedChangeIndicatorDelay());
                }else if (satPhys.maxPoint.magnitude > targetMinMaxRadius.y)
                {
                    setSpeedChangeIndicator = true;
                    speedChangeIndicator.SetActive(true);
                    slowDownImage.SetActive(true);
                    speedChangeText.text = "Slow down here!";
                    speedChangeIndicator.transform.position = satPhys.minPoint;
                    //StartCoroutine(SpeedChangeIndicatorDelay());
                }
                else
                {
                    speedChangeIndicator.SetActive(false);
                    speedUpImage.SetActive(false);
                    slowDownImage.SetActive(false);
                    //setSpeedChangeIndicator = true;
                    //EXPRESSION WHEN THE SATELLITE IS IN THE TARGET!
                }


                //speedChangeIndicator.transform.position
            }
        }
    }

}
