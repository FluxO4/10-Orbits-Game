using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelscreenController : MonoBehaviour
{
    int levelReached;
    public GameObject levelSetPrefab;


    [SerializeField]
    Text numberOfStars;
   /* [SerializeField]
    Color levelSetLockedText;
    [SerializeField]
    Color levelSetUnlockedText;*/

    [SerializeField]
    Color redStar;
    [SerializeField]
    Color orangeStar;
    [SerializeField]
    Color yellowStar;
    [SerializeField]
    Color blueStar;
    [SerializeField]
    Color whiteStar;

    [SerializeField]
    Color[] levelSetThemeColours = new Color[5];

    [SerializeField]
    Slider backgroundSettings;
    [SerializeField]
    SceneController sceneController;
    [SerializeField]
    Material defaultSkybox;
    [SerializeField]
    Material pitchBlackSkybox;
    [SerializeField]
    Material classicSkybox;
    [SerializeField]
    Transform content;
    [SerializeField]
    RectTransform contentR;
    
    [SerializeField]
    List<levelSet> levelSets;

    [HideInInspector]
    public List<GameObject> levelSetObjects;

    [System.Serializable]
    public struct levelSet
    {
        public string title;
        public int unlockStarNum;
    }


    //List<bool> setUnlockState;

    void setUpLevelButtons()
    {
        //PlayerPrefs.GetInt("Level"+levelname+"Score", -1) the playerPref, levelname is the number to-stringed

        //Creating the level sets and buttons
        {
            int totalStars = PlayerPrefs.GetInt("StarsCollected", 0);
            numberOfStars.text = totalStars.ToString();
            int posPointer = 150;
            for(int i = 0; i < levelSets.Count; i++)
            {
                {
                    GameObject temp = Instantiate(levelSetPrefab, content);

                    levelSetObjects.Add(temp);

                }
                levelSetObjects[i].transform.SetParent(content);
                levelSetObjects[i].name = "Level Set " + i.ToString();
                LevelSetProperties t = levelSetObjects[i].GetComponent<LevelSetProperties>();
                t.setnumber = i;
                t.sceneController = sceneController;
                string levelname = (i*10+1).ToString();
                t.Title.text = levelSets[i].title;
                t.Title.color = levelSetThemeColours[i];
                t.UnlockStarNum.text = levelSets[i].unlockStarNum.ToString();
                t.UnlockStarNum.color = levelSetThemeColours[i];
                Image hr = t.transform.Find("HR").GetComponent<Image>();
                hr.color = levelSetThemeColours[i];
                RectTransform r = levelSetObjects[i].GetComponent<RectTransform>();
                r.anchoredPosition += new Vector2(0,-posPointer - 350/2);
                
               

                if(totalStars >= levelSets[i].unlockStarNum)
                {
                    //setUnlockState[i] = true;
                    posPointer += 300;
                    t.UnlockStarNum.text = "Unlocked!";
                    t.lockedBackgroud.SetActive(false);
                    t.unlockedBackground.SetActive(true);
                    Image tempImg = t.unlockedBackground.GetComponent<Image>();
                    tempImg.color = new Color(levelSetThemeColours[i].r, levelSetThemeColours[i].g, levelSetThemeColours[i].b, levelSetThemeColours[i].a/10.0f);

                    for(int ii = 0; ii < 10; ii++)
                    {
                        levelname = (i * 10 + ii + 1).ToString();
                        int score = PlayerPrefs.GetInt("Level" + levelname + "Score", -1);
                        if (score >= 0)
                        {
                            t.levelbuttons[ii].SetActive(true);
                            Text tempImg2 = t.levelbuttons[ii].transform.Find("Text").GetComponent<Text>();
                            Image tempImg3 = t.levelbuttons[ii].transform.Find("Image").GetComponent<Image>();
                            tempImg2.color = levelSetThemeColours[i];
                            tempImg3.color = levelSetThemeColours[i];
                            if (score >= 1)
                            {
                                //t.stars[ii].gameObject.SetActive(true);
                                if (score == 1) {
                                    t.levelbuttons[ii].transform.Find("Star 1").gameObject.SetActive(true);


                                   // t.stars[ii].color = redStar;
                                }else
                                if (score == 2)
                                {
                                    t.levelbuttons[ii].transform.Find("Star 1").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 2").gameObject.SetActive(true);

                                    //t.stars[ii].color = yellowStar;
                                }
                                else
                                if (score == 3)
                                {
                                    t.levelbuttons[ii].transform.Find("Star 1").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 2").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 3").gameObject.SetActive(true);
                                    //t.stars[ii].color = whiteStar;
                                }
                                else
                                if (score == 4)
                                {
                                    t.levelbuttons[ii].transform.Find("Star 1").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 2").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 3").gameObject.SetActive(true);
                                    t.levelbuttons[ii].transform.Find("Star 4").gameObject.SetActive(true);
                                    // t.stars[ii].color = blueStar;
                                }
                                
                            }

                        }
                        else
                        {
                            /* if (ii == 0)
                             {
                                 t.levelbuttons[ii].SetActive(true);
                             }
                             else if (PlayerPrefs.GetInt("Level" + (i * 10 + ii).ToString() + "Score", -1) >= 0) {
                                 t.levelbuttons[ii].SetActive(true);
                             }*/
                            t.levelbuttons[ii].SetActive(true);
                            Text tempImg2 = t.levelbuttons[ii].transform.Find("Text").GetComponent<Text>();
                            Image tempImg3 = t.levelbuttons[ii].transform.Find("Image").GetComponent<Image>();
                            tempImg2.color = levelSetThemeColours[i];
                            tempImg3.color = levelSetThemeColours[i];
                            goto skipp;
                        }
                    }
                skipp:;
                }
                else
                {
                    t.lockedBackgroud.SetActive(true);
                    t.unlockedBackground.SetActive(false);
                    Image tempImg = t.lockedBackgroud.GetComponent<Image>();
                    tempImg.color = new Color(levelSetThemeColours[i].r, levelSetThemeColours[i].g, levelSetThemeColours[i].b, levelSetThemeColours[i].a / 10.0f);
                    posPointer += 80;
                }
                contentR.sizeDelta = new Vector2(0, posPointer + 100);
            }
        }


       /* levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        if (levelReached == 0)
        {
            PlayerPrefs.SetInt("LevelReached", 1);
            PlayerPrefs.Save();
        }
        int levelsplaced = 0;
       for (int i = 1; i <= numberOfLevelSets; i++)
        {
            Transform currentset = content.GetChild(i - 1);
            currentset.gameObject.SetActive(true);

            contentR.sizeDelta = new Vector2(0, 50 + 400 * i);
            
           // RectTransform r = content.GetComponent<RectTransform>();
            //r.rect.Set(r.rect.x,r.rect.y,r.rect.width,r.rect.height+400);

            for (int ii = 1; ii <= 10; ii++)
            {
                currentset.GetChild(ii).gameObject.SetActive(true);
                levelsplaced += 1;
                if (levelsplaced >= levelReached)
                {
                    goto skipp;
                }
            }
        }
    skipp:;*/
    }

    private void Start()
    {
        setBackground();
        setUpLevelButtons();

       
    }

    public void updateBackgroundSettings()
    {
        PlayerPrefs.SetInt("Background", Mathf.RoundToInt(backgroundSettings.value));
        PlayerPrefs.Save();
        if (backgroundSettings.value == 0)
        {
            RenderSettings.skybox = pitchBlackSkybox;
        }
        else
        if (backgroundSettings.value == 1)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
        if (backgroundSettings.value == 2)
        {
            RenderSettings.skybox = classicSkybox;
        }
    }

    void setBackground()
    {
        
        int a = PlayerPrefs.GetInt("Background", 1);
        backgroundSettings.value = a;
        if (a == 0)
        {
            RenderSettings.skybox = pitchBlackSkybox;
        }
        else
        if (a == 1)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
        if (a == 2)
        {
            RenderSettings.skybox = classicSkybox;
        }
    }


    public void resetAll()
    {
        PlayerPrefs.DeleteAll();
        sceneController.reloadScene();
    }
}

