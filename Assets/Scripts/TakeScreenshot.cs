using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    [SerializeField] GameObject blink;
    [SerializeField] GameObject xLine;
    [SerializeField] GameObject yLine;
    [SerializeField] GameObject zLine;
    [SerializeField] GameObject Indicator;
    public PopUpSystem pop;

    public void TakeAScreenshot()
    {
        StartCoroutine("Capture");
    }

	IEnumerator Capture()
	{
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";

        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = false;
        if (xLine.GetComponent<LineRenderer>().enabled)
            xLine.GetComponent<LineRenderer>().enabled = false;
        if (yLine.GetComponent<LineRenderer>().enabled)
            yLine.GetComponent<LineRenderer>().enabled = false;
        if (zLine.GetComponent<LineRenderer>().enabled)
            zLine.GetComponent<LineRenderer>().enabled = false;
        foreach (MeshRenderer x in Indicator.GetComponentsInChildren<MeshRenderer>())
            x.enabled = false;

        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "PG3DScreenshot", fileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);

        GameObject.Find("UICanvas").GetComponent<Canvas>().enabled = true;
        xLine.GetComponent<LineRenderer>().enabled = true;
        yLine.GetComponent<LineRenderer>().enabled = true;
        zLine.GetComponent<LineRenderer>().enabled = true;
        foreach (MeshRenderer x in Indicator.GetComponentsInChildren<MeshRenderer>())
            x.enabled = true;
        Instantiate(blink, new Vector2(0f, 0f), Quaternion.identity);

       
    }
}
