using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using TMPro;

public class TextureCell : MonoBehaviour, ICell
{
    //UI stuff
    public TMP_Text textureNameLabel;
    public Image textureImage;

    private int cellIndex;
    GameObject centerPoint;
    GameObject UICanvas;

    // Start is called before the first frame update
    void Start()
    {
        centerPoint = GameObject.Find("CenterPoint");
        UICanvas = GameObject.Find("UICanvas");
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    //This is called from the SetCell method in DataSource
    public void ConfigureCell(Sprite sprite, int _cellIndex)
    {
        cellIndex = _cellIndex;

        if(_cellIndex == 0)
            textureNameLabel.text = "Original";
        else
            textureNameLabel.text = "Material " + _cellIndex;
        textureImage.sprite = sprite;
        
    }

    private void ButtonListener()
    {
        UICanvas.GetComponent<UIManager>().SetCurrentTexture(cellIndex);
        centerPoint.GetComponent<MaterialModel>().ChangeMaterial(cellIndex);
    }
}
