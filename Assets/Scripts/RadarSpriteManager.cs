using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarSpriteManager : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[6];
    private int state = 0;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = sprites[state - 1];
        }
    }

    public void SetState(int state)
    {
        if (state > 0 && state <= 6)
        {
            this.state = state;
        }
        else
        {
            this.state = 0;
        }
    }
}
