using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class satellitePhysics : MonoBehaviour
{
    //Rigidbody self;
    [HideInInspector]
    public Vector3 speed;

    public bool reportOrbitData = false;

    public int index;

    public Transform satBody; // MAKE THIS ROTATE WHEN SPEED CHANGES, PREFERABLY ANGULAR ACCELERATION

    public Transform boosterHolder;

    public ParticleSystem booster;

    bool changingSpeed;

    bool orbittingBodyUnclear = false;

    Transform orbittingBody;

    float changeSpeedValue;
    float changeAngleValue = 0;

    public event Action destroyed;

    private void OnDestroy()
    {
        destroyed?.Invoke();
    }
    private void Start()
    {
        transform.eulerAngles = new Vector3(0,0,0);
        //Game.SceneObjects.satellite = transform.gameObject;
        //Time.timeScale = 0;
        //boosterHolder = booster.gameObject.transform.parent;
        Game.Controller.speedController.speedUpdated += updateSpeed;
        Game.Controller.speedController.speedZeroed += stopChangeSpeed;
        index = int.Parse(transform.name);
        if (reportOrbitData)
        {
            if(Game.Planets.Count == 1)
            {
                orbittingBody = Game.Planets[0];

            }
            else
            {
                orbittingBodyUnclear = true;
            }
        }

    }

    void OnEnable()
    {
        index = int.Parse(transform.name);
        speed = Vector3.zero;
        changeSpeedValue = 0;
        changingSpeed = false;
        Game.toggles.satInTarget[index] = false;
        if (reportOrbitData)
        {
            recordTime = Time.frameCount;
        }
    }

    private void OnDisable()
    {
        index = int.Parse(transform.name);
        Game.toggles.satInTarget[index] = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "TargetArea")
        {
            Game.toggles.satInTarget[index] = false;
            /*timeInTarget = 0;
            Game.UI.completionIndicator.value = 0;*/
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "TargetArea" && Time.timeSinceLevelLoad > 0.5f)   
        {
            //Debug.Log("HI I'M COLLIDIN, MY NAME IS " + index + " AND THE TIME IS " + Time.time);
            Game.toggles.satInTarget[index] = true;
            /*
            timeInTarget += Time.deltaTime;
            Game.UI.completionIndicator.value = timeInTarget / Game.Constants.targetTimeSec;
            if (timeInTarget >= Game.Constants.targetTimeSec)
            {
                Game.Controller.mainController.winGame();
            }
            */
        }

        if (collision.gameObject.tag == "PlanetBody")
        {
            Instantiate(Game.Prefabs.explosion, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
        else
        if (collision.gameObject.tag == "Satellite")
        {
            Instantiate(Game.Prefabs.explosion, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
        else
        if (collision.gameObject.tag == "Star")
        {
            Instantiate(Game.Prefabs.explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    float minDist = Mathf.Infinity;
    public Vector3 minPoint = Vector3.zero;
    public Vector3 maxPoint = Vector3.zero;
    public Vector3 orbitBodyPos = Vector3.zero;
    float maxDist = 0;
    public bool stableOrbit = false;
    float orbitCheckTime = 0.5f;
    int recordTime = 0;
    int orbitTime=100;
    int prevOrbitTime = 200;
    bool speedButtonsRemoved = false;
    public void resetOrbitData()
    {
         minDist = Mathf.Infinity;
         Vector3 minPoint = Vector3.zero;
         Vector3 maxPoint = Vector3.zero;
         Vector3 orbitBodyPos = Vector3.zero;
         maxDist = 0;
    }
    private void FixedUpdate()
    {
        
        if(transform.position.magnitude > 70)
        {
            Game.Controller.mainController.loseGame();
        }

        

        if (speed.magnitude != 0)
        {
            if (Game.SceneObjects.selectedSat != transform.gameObject)
            {
                goto skipper;
            }
            if (changeSpeedValue < 0)
            {
                if(speed.magnitude < Game.Constants.minSatSpeed)
                {
                    goto skipper;
                }
            }
            else
            {
                if(speed.magnitude > Game.Constants.maxSatSpeed)
                {
                    goto skipper;
                }
            }

            

            if(Game.UI.fuelSlider.value <= 0)
            {
                if (!speedButtonsRemoved)
                {
                    Game.UI.speedcontrollerUI.SetActive(false);
                    speedButtonsRemoved = true;
                    Text infoText = Game.UI.infoText.GetComponent<Text>();
                    infoText.text = "Out of fuel!";
                    booster.Stop();
                }
                goto skipper;
            }

            if (changingSpeed)
            {
                if (Game.UI.fuelSlider.value > 0) {
                    Game.UI.fuelSlider.value += -0.1f;
                }
                speed += changeSpeedValue * speed.normalized * 0.01f;


                if (changeSpeedValue < 0)
                {
                    changeAngleValue += 0.1f;
                    boosterHolder.eulerAngles = new Vector3(0, Vector3.SignedAngle(Vector3.forward, speed, Vector3.up), 0);
                }
                else
                {
                    changeAngleValue -= 0.1f;
                    boosterHolder.eulerAngles = new Vector3(0, Vector3.SignedAngle(Vector3.forward, speed, Vector3.up) + 180, 0);
                }
            }
        skipper:;

            if (changeAngleValue != 0)
            {
                satBody.Rotate(Vector3.up, changeAngleValue, Space.World);
                changeAngleValue *= 0.99f;
            }
            if(Mathf.Abs(changeAngleValue) < 0.01)
            {
                changeAngleValue = 0;
            }
            speed += Forces.Gravity(transform, 60, 0);
            transform.Translate(speed);
        }

        if (reportOrbitData)
        {
            if (!orbittingBodyUnclear)
            {
                float dist = (orbittingBody.transform.position - transform.position).magnitude;

                /*if (Time.time - recordTime > orbitCheckTime)
                {
                    if (Mathf.Abs(minDist - dist) > 0.02f)
                    {
                        //stableOrbit = true;
                        //Debug.Log("STABLE ORBIT");
                        orbitTime = Time.time - recordTime;
                        recordTime = Time.time;
                    }
                }*/

                /*if (Time.frameCount - recordTime > orbitCheckTime / Time.deltaTime)
                {
                    if ((transform.position - minPoint).magnitude < 0.1f)
                    {

                        orbitTime = Time.frameCount - recordTime;
                        if (Mathf.Abs(orbitTime - prevOrbitTime) < 10)
                        {
                            if (!stableOrbit)
                            {
                                stableOrbit = true;
                                //Debug.Log("STABLE ORBIT!");
                            }
                        }
                        else
                        {
                            if (stableOrbit)
                            {
                                stableOrbit = false;
                                //Debug.Log("ORBIT DESTABILISED!");
                            }
                        }
                        prevOrbitTime = orbitTime;
                        //Debug.Log("OrbitTime = " + orbitTime);
                        recordTime = Time.frameCount;
                    }
                }*/


                if (dist < minDist)
                {
                    minDist = dist;
                    minPoint = transform.position;

                }


                if (dist > maxDist)
                {
                    maxDist = dist;
                    maxPoint = transform.position;
                }
            }
        }
    }

    public void addVelocity(Vector3 vel)
    {
        speed += vel;
    }

    public void updateSpeed()
    {
        if (this)
        {
            {
                changeSpeed(Game.UI.speedSlider.value);
            }
        }
    }

    public void changeSpeed(float accelerationValue)
    {
        if (Game.SceneObjects.selectedSat == transform.gameObject)
        {
            changingSpeed = true;
            booster.Play();
            changeSpeedValue = accelerationValue * Game.Constants.speedChangeFactor;
           // Debug.Log(Game.Constants.speedChangeFactor);
        }
    }

    public void stopChangeSpeed()
    {
        if (this)
        {
            //if (Game.Selection.selectedObject == transform.gameObject)
            {

                changingSpeed = false;
                if (Game.SceneObjects.selectedSat == transform.gameObject)
                {
                    booster.Stop();
                }
            }
        }
    }
}

