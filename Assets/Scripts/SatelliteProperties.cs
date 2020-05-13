using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteProperties : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(1,10)]
    public int launchOrder = 1;
}
