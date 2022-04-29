using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


public class BluetoothController : MonoBehaviour
{
    public FixedJoystick leftStick;
    public FixedJoystick rightStick;
    public GameObject ScannerControllerObject;

    public Text text;

    private ScannerController scannerController;

    private int leftSpeed;
    private int rightSpeed;

    public String deviceName;
    private bool IsConnected;
    public static string dataRecived = "";



    void Start()
    {
        scannerController = ScannerControllerObject.GetComponent<ScannerController>();

        BluetoothService.CreateBluetoothObject();

        IsConnected =  BluetoothService.StartBluetoothConnection(deviceName);

        if(IsConnected){
            text.text = "Connected";
        }else{
            text.text = "Not Connected";

        }

    }


    private int oldLeft = 0;
    private int oldRight = 0;
    // Update is called once per frame
    void Update()
    {
        leftSpeed = (int) Math.Round(leftStick.Vertical*3)*100;
        rightSpeed = (int) Math.Round(rightStick.Vertical*3)*100;

        if (IsConnected) {

           
           
            if(leftSpeed != oldLeft || rightSpeed != oldRight){
                String cmd = "m " + leftSpeed + " " + rightSpeed + ";";
                text.text = cmd;
                BluetoothService.WritetoBluetooth(cmd);

                oldLeft = leftSpeed;
                oldRight = rightSpeed;
            }


            try
            {
                string datain =  BluetoothService.ReadFromBluetooth();
                if (datain.Length > 1)
                {   
                    if(datain.Contains("sd")){
                        string[] sd = datain.Split(' ');
                        scannerController.SetScanner(Int32.Parse(sd[1]),Int32.Parse(sd[2]),Int32.Parse(sd[3]),Int32.Parse(sd[4]));
                    }
                }
        
            }
            catch (Exception e)
            {
            }
        }
    }



    
}

