using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabList : MonoBehaviour
{
    public GameObject satellite;
    public GameObject launchRing;
    public GameObject launchArrow;
    public GameObject explosion;
    public GameObject levelSetPrefab;
    private void Awake()
    {
        Game.SceneObjects.satelliteP = satellite;
        Game.Prefabs.explosion = explosion;
        Game.SceneObjects.launchArrow = launchArrow;
        Game.SceneObjects.launchRing = launchRing;
        Game.Prefabs.levelSetPrefab = levelSetPrefab;
    }
}
