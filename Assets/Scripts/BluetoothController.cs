using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluetoothController : MonoBehaviour
{
    public FixedJoystick leftStick;
    public FixedJoystick rightStick;
    public GameObject ScannerControllerObject;

    private ScannerController scannerController;

    private int leftSpeed;
    private int rightSpeed;

    void Start()
    {
        scannerController = ScannerControllerObject.GetComponent<ScannerController>();
    }


    // Update is called once per frame
    void Update()
    {
        leftSpeed = (int)leftStick.Vertical * 500;
        rightSpeed = (int)rightStick.Vertical * 500;

    }
}
