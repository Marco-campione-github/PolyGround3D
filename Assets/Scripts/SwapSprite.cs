using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapSprite : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    bool original = true;
    public void ChangeSprite()
    {
        if (original)
        {
            GetComponent<Button>().image.sprite = sprite2;
            original = false;
        }
        else
        {
            GetComponent<Button>().image.sprite = sprite1;
            original = true;
        }
            
    }
}
