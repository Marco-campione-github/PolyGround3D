using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ObjFromStream : MonoBehaviour {
	void Start () {
        //make unity web request
        var loaded = new UnityWebRequest("https://people.sc.fsu.edu/~jburkardt/data/obj/lamp.obj");
        loaded.downloadHandler = new DownloadHandlerBuffer();
        loaded.SendWebRequest();
        
        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(loaded.downloadHandler.text));
        var loadedObj = new OBJLoader().Load(textStream);
	}
}
