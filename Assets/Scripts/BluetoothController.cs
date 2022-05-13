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

    // BUTTONS
    public Button reconnectBtn;


    // RADAR CONTROLLER SCRIPT
    private RadarController radarController;

    void Connect()
    {
        // GENERATE BLUETOOTH OBJECT
        BluetoothService.CreateBluetoothObject();
        isConnected = BluetoothService.StartBluetoothConnection(deviceName);
        UpdateConnection();
        DebugLog("Try reconnect: " + ((isConnected) ? "success" : "failed"));
    }

    void UpdateConnection(){
        connectionText.text = (isConnected) ? "Connected" : "Click to connect";
        connectionText.color = (isConnected) ? Color.green : Color.red;
    }

    void Start()
    {
        // KEEP DISPLAY ALWAYS ON
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // GET RADARCONTROLLER SCRIPT
        radarController = RadarControllerObject.GetComponent<RadarController>();

        // GET BUTTON OBJECT
        reconnectBtn.onClick.AddListener(Connect);

        // CHECK IF IS CONNECTED AND DISPLAY STATUS
        Connect();
        debugLogText.enabled = debugMode;
    }

    // JOYSTICK SPEED
    private int leftSpeed;
    private int rightSpeed;

    private int noData;


    float time;
    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {

            time = time + 1f * Time.deltaTime;
            if (time >= 0.3f)
            {


                // CONVERT JOYSTICK DATA TO SPEED VALUES (0-300, 100 Steps)
                leftSpeed = ((int)Math.Round(leftStick.Vertical * 3) * 100) + 300;
                rightSpeed = ((int)Math.Round(rightStick.Vertical * 3) * 100) + 300;


                // WHEN SPEED CHANGES SEND NEW DATA
                String cmd = "m " + leftSpeed.ToString("000") + " " + rightSpeed.ToString("000") + ";";
                try
                {
                    BluetoothService.WritetoBluetooth(cmd);
                }
                catch (Exception e)
                {
                    DebugLog(e.Message);
                    isConnected = false;
                }

                try
                {
                    String bluetoothData = BluetoothService.ReadFromBluetooth();
                    if (bluetoothData.Length > 0)
                    {   
                        noData = 0;
                        if (bluetoothData.StartsWith("sd"))
                        {
                            string[] sd = bluetoothData.Split(' ');
                            if (sd.Length >= 5)
                            {
                                radarController.SetScanner(Int32.Parse(sd[1]), Int32.Parse(sd[2]), Int32.Parse(sd[3]), Int32.Parse(sd[4]));
                            }
                        }
                    }else
                    {
                        noData++;

                    }
                }
                catch (Exception e)
                {
                    DebugLog(e.Message);
                    isConnected = false;
                }
                time = 0;
            }
        }
        if(noData > 10){
            isConnected = false;
            noData = 0;
            UpdateConnection();
        }
    }

    void DebugLog(String log)
    {
        if (debugMode)
        {
            debugLogText.text += "\n" + log;
        }
    }



}

