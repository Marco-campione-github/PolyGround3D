using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }
public class ColorPicker : MonoBehaviour
{
    //Events
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;
    public Camera UICamera;
    RectTransform rect;
    Texture2D colorTexture;
    Color color;
    //public Light dirLight;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        colorTexture = GetComponent<Image>().mainTexture as Texture2D;
        color = Color.white;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.GetTouch(0).position, UICamera, out Vector2 delta);

            float width = rect.rect.width;
            float height = rect.rect.height;
            delta += new Vector2(width * .5f, height * .5f);

            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);

            int texX = Mathf.RoundToInt(x * colorTexture.width);
            int texY = Mathf.RoundToInt(y * colorTexture.height);

            if (!(delta.x < 0f || delta.x > width || delta.y < 0f || delta.y > height))
            {
                color = colorTexture.GetPixel(texX, texY);
                OnColorPreview?.Invoke(color); //cambia il colore della preview
                OnColorSelect?.Invoke(color); //cambia il colore del modello
            }
            
        }
        
    }
}
