using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyScript : MonoBehaviour
{
    public float frequency;

    private RadioScript radioScript;
    public bool inRange = false;
    public GameObject wall;
    private bool bUnlocked = false;

    void Start()
    {
        
    }

    void Update()
    {
        
        if (inRange && !bUnlocked)
        {
            radioScript.targetFrequency = frequency;
            //radioScript.inRangeFrequency = true;
        }
        else if(inRange && bUnlocked)
        {
            radioScript.inRangeFrequency = false;
        }
        else if(radioScript != null)
        {
            //radioScript.inRangeFrequency = false;
        }
    }

    public void unlocked()
    {
        bUnlocked = true;
        wall.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Radio")
        {
            radioScript = other.gameObject.GetComponentInParent<RadioScript>();
            radioScript.frequencyScript = this;
            inRange = true;
            radioScript.inRangeFrequency = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Radio")
        {
            inRange = false;
            radioScript.inRangeFrequency = false;
        }
    }
}
