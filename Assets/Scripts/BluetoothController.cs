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

    // CONTROLLER
    public FixedJoystick joystick;
    public Button speedBtn;
    public Button horn;

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

    void UpdateConnection()
    {
        connectionText.text = (isConnected) ? "Connected" : "Click to connect";
        connectionText.color = (isConnected) ? Color.green : Color.red;
    }

    public void ChangeSpeed()
    {
        speedMultiplier = (speedMultiplier == 1) ? 2 : 1;
        speedBtn.GetComponent<Image>().color = (speedMultiplier == 1) ? Color.white : Color.red;
        DebugLog(speedMultiplier.ToString());
    }

    public void Honk()
    {
        String cmd = "h;";
        DebugLog(cmd);
        try
        {
            BluetoothService.WritetoBluetooth(cmd);
        }
        catch (Exception e)
        {
            DebugLog(e.Message);
            isConnected = false;
        }
    }


    private int speedMultiplier = 1;

    void Start()
    {
        // KEEP DISPLAY ALWAYS ON
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // GET RADARCONTROLLER SCRIPT
        radarController = RadarControllerObject.GetComponent<RadarController>();

        // BUTTON LISTENER
        reconnectBtn.onClick.AddListener(Connect);
        speedBtn.onClick.AddListener(ChangeSpeed);
        horn.onClick.AddListener(Honk);

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

                //joystick.Horizontal; 0
                //joystick.Vertical; 1 ==> 300
                int max = 250;
                int steps = 50;
                leftSpeed = (speedMultiplier * (int)Math.Round(joystick.Vertical * (max / steps)) * steps) + ((int)Math.Round(joystick.Horizontal * (max / steps)) * steps) + 300;
                rightSpeed = (speedMultiplier * (int)Math.Round(joystick.Vertical * (max / steps)) * steps) + ((int)Math.Round(-joystick.Horizontal * (max / steps)) * steps) + 300;

                // CONVERT JOYSTICK DATA TO SPEED VALUES (0-300, 100 Steps)
                //leftSpeed = ((int)Math.Round(leftStick.Vertical * 3) * 100) + 300;
                //rightSpeed = ((int)Math.Round(rightStick.Vertical * 3) * 100) + 300;


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
                    }
                    else
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
        if (noData > 10)
        {
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

