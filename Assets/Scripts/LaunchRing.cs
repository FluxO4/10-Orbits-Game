using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LaunchRing : MonoBehaviour
{
    public float LaunchStrength = 0.05f;
    [Range(0,360)]
    public float launchAngle = 20;
    public Vector3 launchVector = new Vector3(1,0,0);

    float radius = 3.1f;
    float scaleFactor = 1;
    bool firstClick = true;

    bool started = true;
    string temp;
    public event Action LaunchSat;
    public event Action ClickInCircle;
    private void Start()
    {
        LaunchStrength = launchVector.magnitude * 0.1f / scaleFactor;
        launchAngle = Vector3.SignedAngle(Vector3.right, launchVector, Vector3.up);
        Game.toggles.selectingLaunchAngle = false;
        Text t = Game.UI.infoText.GetComponent<Text>();
        temp = t.text;

    }
    void Update()
    {

        scaleFactor = Game.Controller.cameraMovementr.orthroSize;
        radius = scaleFactor * 0.7f;
        
        transform.position = Game.SceneObjects.selectedSat.transform.position + new Vector3(0, -10, 0);
        transform.localScale = Vector3.one * scaleFactor * 0.3f;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            Vector3 clickPoint = Game.SceneObjects.MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60));
            Vector3 rposition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 tlaunchVector = clickPoint - rposition;
            // if ((Game.SceneObjects.MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60)) - new Vector3(transform.position.x, 0, transform.position.z)).magnitude < radius)
            if (tlaunchVector.magnitude < radius)
            {
                if (!started) { started = true;
                    
                }
                Game.toggles.selectingLaunchAngle = true;
            }
            //Game.Controller.cameraMovementr.starteddragging = false;
        }

        if (Input.GetMouseButton(0) && Game.toggles.selectingLaunchAngle)
        {
            Vector3 clickPoint = Game.SceneObjects.MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60));
            Vector3 rposition = new Vector3(transform.position.x, 0, transform.position.z);
            //Debug.DrawLine(clickPoint, rposition, Color.white, 1);
            launchVector = clickPoint - rposition;
            if(launchVector.magnitude > radius)
            {
                launchVector = launchVector.normalized * radius;
            }
            LaunchStrength = launchVector.magnitude * 0.1f/ scaleFactor;
            launchAngle = Vector3.SignedAngle(Vector3.right, launchVector, Vector3.up);

            /*if (launchVector.magnitude > radius)
            {
                Game.toggles.selectingLaunchAngle = false;
            }*/

        }

        if (started)
        {
            Game.SceneObjects.launchArrow.transform.eulerAngles = new Vector3(0, launchAngle, 0);
            Game.SceneObjects.launchArrow.transform.localScale = Vector3.one * LaunchStrength * 26f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (firstClick)
            {
                if (Game.toggles.selectingLaunchAngle)
                {
                    ClickInCircle?.Invoke();
                    firstClick = false;
                }
                
            }

            Game.toggles.selectingLaunchAngle = false;
            //Game.Controller.cameraMovementr.starteddragging = false;
        }
    }

    public void launch()
    {
        if (started) {
            LaunchSat?.Invoke();

            Text tt = Game.UI.infoText.GetComponent<Text>();
            tt.text = temp;
            //Debug.Log("LAUNCHED!");
            Game.Controller.cameraMovementr.cameraShake();
            satellitePhysics t = Game.SceneObjects.selectedSat.GetComponent<satellitePhysics>();
            t.enabled = true;
            t.addVelocity(launchVector.normalized * LaunchStrength * Game.Constants.satLaunchFactor);
            Game.UI.speedcontrollerUI.SetActive(true);
            Game.UI.launchButton.SetActive(false);
            if (Game.Controller.mainController.satellitesDone < Game.SceneObjects.satellites.Count-1)
            {
                Game.UI.doneButton.SetActive(true);
            }
            Game.UI.completionIndicator.gameObject.SetActive(true);
            
            transform.gameObject.SetActive(false);
        }
        else
        {
            Text t = Game.UI.infoText.GetComponent<Text>();
            t.text = "Tap in the launch circle to select an angle and a speed!";
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


}

