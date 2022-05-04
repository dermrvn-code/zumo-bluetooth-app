using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarSpriteManager : MonoBehaviour
{
    // ARRAY OF DIFFERENT STATE SPRITES
    public Sprite[] sprites = new Sprite[6];
    private Image image;

    void Start()
    {
        // GET THE IMAGE OBJECT THE SCRIPT IS PLACED ON
        image = GetComponent<Image>();
    }

    // SET STATE AS A PUBLIC FUNCTION
    public void SetState(int state)
    {
        // ENABLE IMAGE AND CHANGE SPRITE TO STATE SPRITE
        if (state > 1 && state <= 6)
        {
            image.enabled = true;
            image.sprite = sprites[state - 1];
        }
        else
        {
            // DISBLE IMAGE IF STATE IS 0 OR UNVALID
            image.enabled = false;
        }
    }
}
