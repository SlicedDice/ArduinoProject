using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

using StarterAssets;

public class ArduinoScript : MonoBehaviour
{
    private string port = "COM4";
    public int baudrate = 9600;
    SerialPort arduinoPort;
    private bool isStreaming = false;

    public RadioScript radioScript;
    public PlatformScript platformScript;
    
    private string[] portInput;
    string tmp;

    
    void Start()
    {
        OpenConnection();
        InvokeRepeating("ReadSerialPort", 0f, 0.1f);
    }

    void FixedUpdate()
    {
        if (isStreaming)
        {
             //ReadSerialPort();
            if(tmp != null) Debug.Log(tmp);
            if(tmp != null && tmp != "")
            {
                //portInput = splitString(tmp, '.');
                portInput = tmp.Split('.');
                if (portInput.Length == 3)
                {
                    
                    radioScript.assignValue(portInput[0]);
                    radioScript.active = true;
                    platformScript.assignValues(portInput[1], portInput[2]);
                    platformScript.active = true;
                }
                tmp = null;
            }
            
        }
    }
    
    private string[] splitString(string input, char delimiter)
    {
        string[] tmpArray = new string[3];
        int ctr = 0;
        string tmpStr = "";
        while(input[ctr] != delimiter)
        {
            tmpStr += input[ctr];
            ctr++;
        }
        tmpArray[0] = tmpStr;
        ctr++;
        tmpStr = "";
        while(input[ctr] != delimiter)
        {
            tmpStr += input[ctr];
            ctr++;
        }
        tmpArray[1] = tmpStr;
        ctr++;
        tmpStr = "";
        while(ctr < input.Length)
        {
            tmpStr += input[ctr];
            ctr++;
        }
        tmpArray[2] = tmpStr;

        return tmpArray;
    }
    public void WriteToLED(float state)
    {
        arduinoPort.WriteLine("S" + state.ToString());
    }

    void OpenConnection()
    {
        isStreaming = true;

        arduinoPort = new SerialPort(port, baudrate);

        arduinoPort.ReadTimeout = 100; 
        arduinoPort.Open();
    }
    void Close()
    {
        arduinoPort.Close();
    }
    string ReadSerialPort()
    {
        string message;
        arduinoPort.ReadTimeout = 5;

        try
        {
            message = arduinoPort.ReadLine();
            tmp = message;
            return message;
        } catch (TimeoutException)
        {
            return null;
        }
    }
    
}
