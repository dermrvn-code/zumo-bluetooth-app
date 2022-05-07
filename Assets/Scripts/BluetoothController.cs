using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


public class BluetoothController : MonoBehaviour
{
    // DEBUG MODE
    public bool debugMode;

    // GET JOYSTICKS
    public FixedJoystick leftStick;
    public FixedJoystick rightStick;

    // GET RADAR CONTROLLER OBJECT
    public GameObject RadarControllerObject;

    // GET DISPLAY TEXT
    public Text connectionText;
    public Text debugLogText;

    // BLUETOOTH VARIABLES
    public String deviceName;
    private bool isConnected;


    // RADAR CONTROLLER SCRIPT
    private RadarController radarController;


    void Start()
    {
        // KEEP DISPLAY ALWAYS ON
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // GET RADARCONTROLLER SCRIPT
        radarController = RadarControllerObject.GetComponent<RadarController>();

        // GENERATE BLUETOOTH OBJECT
        BluetoothService.CreateBluetoothObject();

        // CHECK IF IS CONNECTED AND DISPLAY STATUS
        isConnected = BluetoothService.StartBluetoothConnection(deviceName);
        connectionText.text = (isConnected) ? "Connected to " + deviceName : "Not Connected";
        connectionText.color = (isConnected) ? Color.green : Color.red;
    }

    // JOYSTICK SPEED
    private int leftSpeed;
    private int rightSpeed;

    // OLD JOYSTICK SPEED FOR CHANGE COMPARISON
    private int oldLeftSpeed = 0;
    private int oldRightSpeed = 0;

    float time;
    // Update is called once per frame
    void Update()
    {
        time = time + 1f * Time.deltaTime;

        if (time >= 0.2f) {

            debugLogText.enabled = debugMode;

            if (isConnected)
            {
                // CONVERT JOYSTICK DATA TO SPEED VALUES (0-300, 100 Steps)
                leftSpeed = ((int)Math.Round(leftStick.Vertical * 3) * 100) + 300;
                rightSpeed = ((int)Math.Round(rightStick.Vertical * 3) * 100) + 300;


                // WHEN SPEED CHANGES SEND NEW DATA
                /*if (leftSpeed != oldLeftSpeed || rightSpeed != oldRightSpeed)
                {*/
                    String cmd = "m " + leftSpeed + " " + rightSpeed + ";";
                    try
                    {
                        BluetoothService.WritetoBluetooth(cmd);
                    }
                    catch (Exception e)
                    {
                        DebugLog(e.Message);
                    }

                    // UPDATE THE OLD SPEED
                    oldLeftSpeed = leftSpeed;
                    oldRightSpeed = rightSpeed;
                //}


                try
                {   
                    String bluetoothData = BluetoothService.ReadFromBluetooth();
                    if (bluetoothData.Length > 0)
                    {
                        if (bluetoothData.StartsWith("sd"))
                        {
                            string[] sd = bluetoothData.Split(' ');
                            if(sd.Length >= 5){
                                radarController.SetScanner(Int32.Parse(sd[1]), Int32.Parse(sd[2]), Int32.Parse(sd[3]), Int32.Parse(sd[4]));
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    DebugLog(e.Message);
                }
            }
            time = 0;

        }
    }

    void DebugLog(String log)
    {
        if(debugMode){
            debugLogText.text += "\n" + log;
        }
    }



}

