using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetProperties : MonoBehaviour
{
    [HideInInspector]
    public int setnumber;
    public List<GameObject> levelbuttons;
    //public List<GameObject> stars;
    //public List<Image> stars;

 
    public Text UnlockStarNum;
    public Text Title;
    public GameObject lockedBackgroud;
    public GameObject unlockedBackground;

    public SceneController sceneController;

    public void loadButtonLevel(GameObject self)
    {
        int n = setnumber * 10 + int.Parse(self.name);
        sceneController.loadLevel((n).ToString());
    }
}
