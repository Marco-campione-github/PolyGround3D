using System.Collections;
using UnityEngine;

public class IconMaker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Texture2D ToTexture2D(RenderTexture rTex)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rTex;
        Camera.current.Render();
        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rTex.width, rTex.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}
