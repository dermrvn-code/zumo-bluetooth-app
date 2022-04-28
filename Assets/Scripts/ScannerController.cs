using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerController : MonoBehaviour
{

    public Image frontLeft;
    public Image frontRight;
    public Image sideLeft;
    public Image sideRight;

    private int frontLeftValue = 0;
    private int frontRightValue = 0;
    private int sideLeftValue = 0;
    private int sideRightValue = 0;

    private RadarSpriteManager frontLeftScript;
    private RadarSpriteManager frontRightScript;
    private RadarSpriteManager sideLeftScript;
    private RadarSpriteManager sideRightScript;

    // Start is called before the first frame update
    void Start()
    {
        frontLeftScript = frontLeft.GetComponent<RadarSpriteManager>();
        frontRightScript = frontRight.GetComponent<RadarSpriteManager>();
        sideLeftScript = sideLeft.GetComponent<RadarSpriteManager>();
        sideRightScript = sideRight.GetComponent<RadarSpriteManager>();
    }

    // Update is called once per frame
    void Update()
    {
        frontLeftScript.SetState(frontLeftValue);
        frontRightScript.SetState(frontRightValue);

        sideLeftScript.SetState(sideLeftValue);
        sideRightScript.SetState(sideRightValue);
    }

    public void SetScanner(int fl, int fr, int sl, int sr)
    {
        frontLeftValue = fl;
        frontRightValue = fr;
        sideLeftValue = sl;
        sideRightValue = sr;
    }
}
