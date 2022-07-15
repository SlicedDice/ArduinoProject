using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public bool active;

    private Vector3 needlePosition;
    public float fValue;
    private string value = "0";

    public GameObject radioNeedle;
    public GameObject radioKnob;

    public float radioNeedleMin;
    public float radioNeedleMax;
    public float radioStations;
    private float distBtwStations;
    private float radioRotSteps;


    public FrequencyScript frequencyScript;
    public float targetFrequency = 0f;
    public float frequencyTimer = 2.5f;
    public bool inRangeFrequency = false;

    public ArduinoScript arduinoScript;

    // Start is called before the first frame update
    void Start()
    {
        distBtwStations = (-radioNeedleMin + radioNeedleMax) / radioStations;
        radioRotSteps = 360 / radioStations;
        needlePosition = radioNeedle.transform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (value != null)
            {
                changeRadio(value);
                if(inRangeFrequency) frequencyCheck();
                if (!inRangeFrequency) frequencyCheck(true);
            }
        }
        
    }
    private void frequencyCheck(bool reset = false )
    {
        if (reset)
        {
            arduinoScript.WriteToLED(9f);
        }
        else
        {
            float tmp = targetFrequency - fValue;
            if (tmp < 0f) tmp *= -1f;

            if (tmp < 10f)
            {
                frequencyTimer -= Time.deltaTime / (tmp + 1f);
                arduinoScript.WriteToLED(tmp);
            }
            if (tmp > 10f)
            {
                arduinoScript.WriteToLED(9f);
            }
            if (frequencyTimer <= 0f)
            {
                frequencyScript.unlocked();
                frequencyTimer = 2.5f;
            }
        }
        

        
    }
    public void changeRadio(string value)
    {
        if (value != null && float.TryParse(value, out fValue))
        {
            fValue = float.Parse(value);
            float tmp;
            if ((fValue * distBtwStations) > radioNeedleMax) tmp = radioNeedleMax;
            else if ((fValue * distBtwStations) < radioNeedleMin) tmp = radioNeedleMin;
            else tmp = fValue * distBtwStations;

            float tmpRot = fValue * radioRotSteps;

            radioKnob.transform.localEulerAngles = new Vector3(0f, 0f, -tmpRot);
            radioNeedle.transform.localPosition = new Vector3(tmp, needlePosition.y, needlePosition.z);
        }
        
    }

    public void assignValue(string pValue)
    {
        value = pValue;
    }
}
