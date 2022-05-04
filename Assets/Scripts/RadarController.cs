using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarController : MonoBehaviour
{
    // ALL THE RADAR OVERLAY IMAGES
    public Image frontLeft;
    public Image frontRight;
    public Image sideLeft;
    public Image sideRight;

    // SCRIPTS LAYING ON THE IMAGE OBJECTS
    private RadarSpriteManager frontLeftScript;
    private RadarSpriteManager frontRightScript;
    private RadarSpriteManager sideLeftScript;
    private RadarSpriteManager sideRightScript;

    void Start()
    {
        // GET SCRIPT FROM IMAGE OBJECT
        frontLeftScript = frontLeft.GetComponent<RadarSpriteManager>();
        frontRightScript = frontRight.GetComponent<RadarSpriteManager>();
        sideLeftScript = sideLeft.GetComponent<RadarSpriteManager>();
        sideRightScript = sideRight.GetComponent<RadarSpriteManager>();
    }

    public void SetScanner(int fl, int fr, int sl, int sr)
    {
        frontLeftScript.SetState(fl);
        frontRightScript.SetState(fr);
        sideLeftScript.SetState(sl);
        sideRightScript.SetState(sr);
    }
}
