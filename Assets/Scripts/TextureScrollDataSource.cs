using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;

public class TextureScrollDataSource : MonoBehaviour, IRecyclableScrollRectDataSource
{

    public GameObject centerPoint;
    List<Sprite> textures;

    [SerializeField]
    RecyclableScrollRect _recyclableScrollRect;

    private void Awake()
    {
        textures = new List<Sprite>();
        InitData();
        _recyclableScrollRect.DataSource = this;
    }

    private void InitData()
    {
        Object[] objs = ResourceLoader.getMaterials();
        if (objs != null){
            textures.Add(ResourceLoader.getRenderByName("user_model"));
            for(int i = 0; i < objs.Length; i++)
            {
                Material mat = objs[i] as Material;
                AddTextureFromMaterial(mat);
            }
        }
        
    }

    public int GetItemCount()
    {
        return textures.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        var item = cell as TextureCell;
        item.ConfigureCell(textures[index], index);
    }

    public void AddTextureFromMaterial(Material mat)
    {
        Texture2D tex = mat.mainTexture as Texture2D;
        Sprite newSprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
        textures.Add(newSprite);
    }

}
