

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class cameraMovement : MonoBehaviour
{
    //To check if screen is being touched by two fingers so dragging can be skipped
    private bool isZooming = false;

    //size inherited from camera, changed while zooming
    public float orthroSize;
    float screenFactor;

    //Camera component to extract orthroSize from
    Camera MainCam;

    //pointer's position on screen, for the maths, could change when maths is changed
    Vector3 newPos;
    Vector3 deltaPos;
    Vector3 lastPos;
    //Zooming speed, linear, could change when I need to test on touch screens

    //Value of pi, more accurate than Mathf.Pi
    float pi = 3.141592653589793f;

    //Camera's y angle, could change when maths is changed
    float theta;

    //Turns on when dragging is started from screen area outside the side panel, so screen can't be dragged when mousebuttondown is on side panel
    public bool starteddragging;

    [SerializeField]
    float zoomOutLimit = 50;
    [SerializeField]
    float zoomInLimit = 2;


    //Start frame is called once before the first frame


    void Start()
    {
        Input.simulateMouseWithTouches = true;
        lastPos = Vector3.zero;
        newPos = Vector3.zero;
        deltaPos = Vector3.zero;
        starteddragging = false;
        //Extracting camera component
        MainCam = transform.GetComponent<Camera>();

        screenFactor = 1280.0f / (Screen.width * 1f);
        //Extracting the orthroSize
        // orthroSize = MainCam.fieldOfView;
        orthroSize = MainCam.orthographicSize;
    }

    private void OnEnable()
    {
        lastPos = Input.mousePosition;
        starteddragging = false;
    }
    void LateUpdate()
    {
        //Debug.Log(Game.toggles.selectingLaunchAngle);
        if (Game.toggles.selectingLaunchAngle)
        {
            goto skipper;
        }
        if (Input.touchCount == 2 && !IsPointerOverUIObject())
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);


            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;


            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;


            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

             if (deltaMagnitudeDiff < 0)
             {
                 orthroSize = orthroSize + Game.Constants.cameraZoomFactor * MainCam.orthographicSize * deltaMagnitudeDiff / (Screen.height * 1.0f);
                 if (orthroSize < zoomInLimit) { orthroSize = zoomInLimit; }
                 MainCam.orthographicSize = orthroSize;
             }
             else
             {
                 orthroSize = orthroSize + Game.Constants.cameraZoomFactor * MainCam.orthographicSize * deltaMagnitudeDiff / (Screen.height * 1.0f);
                 if (orthroSize > zoomOutLimit) { orthroSize = zoomOutLimit; }
                 MainCam.orthographicSize = orthroSize;
             }


            lastPos = Input.mousePosition;
            isZooming = true;
        }


        //Screen Movement

        if (Input.GetMouseButtonUp(0))
        {
            starteddragging = false;
            isZooming = false;
        }

        //In case zooming with touch screen is in progress, skip to end of update function
        if (isZooming)
        {
            goto skipper;
        }


        if (Input.GetKey(","))
        {
            orthroSize = orthroSize - Game.Constants.cameraZoomFactor * MainCam.orthographicSize * 0.01f * screenFactor;
            if (orthroSize < zoomInLimit) { orthroSize = zoomInLimit; }
            MainCam.orthographicSize = orthroSize;
        }
        else if (Input.GetKey("."))
        {
            orthroSize = orthroSize + Game.Constants.cameraZoomFactor * MainCam.orthographicSize * 0.01f * screenFactor;
            if (orthroSize > zoomOutLimit) { orthroSize = zoomOutLimit; }
            MainCam.orthographicSize = orthroSize;
        }

        if (Input.GetMouseButtonUp(0))
        {
            starteddragging = false;

        }


        //On mouse down, it is detected if mouse is on side panel, and the screen movement is only allowed when it's not
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            // Debug.Log("CAMERA MOVEMENT: MOUSE BUTTON DOWN TRIGGER" + Time.time);
            starteddragging = true;
            // Debug.Log(starteddragging + "" + Time.time);
            //last position
            lastPos = Input.mousePosition;
        }

        //On mouse button, screen moves linearly for now, I could change it
        if (Input.GetMouseButton(0))

        {
            deltaPos = Input.mousePosition - lastPos;

            if (starteddragging)
            { // NEED TO CHGANGE HOW THIS IS DONE
                float multiplier = -(orthroSize * 2f * Game.Constants.cameraMoveFactor) / (Screen.height * 1.0f);
                theta = transform.eulerAngles.y * Mathf.PI / 180;
                transform.position = transform.position + new Vector3(deltaPos.y * Mathf.Sin(theta) * multiplier, 0, deltaPos.y * Mathf.Cos(theta) * multiplier);
                transform.position = transform.position + new Vector3(deltaPos.x * Mathf.Sin(theta + Mathf.PI / 2) * multiplier, 0, deltaPos.x * Mathf.Cos(theta + Mathf.PI / 2) * multiplier);
            }


            //Last position
            lastPos = Input.mousePosition;
        }




    //End
    skipper:;

        if (isShaking)
        {
            shakeAngle = (shakeAngle + 0.5f);
            float x = 0.02f*Mathf.Sin(shakeAngle) / shakeAngle;
            shakeDelta = new Vector3(x, 0, 0);
            transform.position += shakeDelta;
            if (shakeAngle < 0.01f)
            {
                isShaking = false;
            }
        }
    }

    bool isShaking = false;
    Vector3 shakeDelta;
    float shakeAngle = 0;
    public void cameraShake()
    {
        shakeAngle = 0.1f;
        isShaking = true;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    //Converters for the maths, before I realised they existed on Mathf
    float deg(float radians)
    {
        return radians * 180 / pi;
    }

    float rad(float degrees)
    {
        return degrees * pi / 180;
    }
}
