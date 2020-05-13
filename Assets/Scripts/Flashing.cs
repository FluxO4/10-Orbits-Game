using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour
{
    float averageSize = 1;
    //float timeConstant = 1;
    public float amplitude = 0.05f;
    public float frequency = 5;
    // Start is called before the first frame update
    void Start()
    {
        //timeConstant = Time.deltaTime;
        averageSize = transform.localScale.x;
        StartCoroutine(localUpdate());
    }

    // Update is called once per frame
    
    IEnumerator localUpdate()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.02f);
            transform.localScale = Vector3.one * (averageSize + amplitude * Mathf.Sin(1.5f*frequency * Time.time));
        }
    }



}
