using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnModel : MonoBehaviour
{
    public GameObject model;
    private void Awake()
    {
        //Spawns a pre-loaded model
        if (MainMenu.loadingMethod == 0) {
            model = MainMenu.resources[MainMenu.openedModelIndex] as GameObject;
            Instantiate(model, transform.position, Quaternion.identity, transform);
        }
        
        //Spawns a runtime-loaded model without materials
        if(MainMenu.loadingMethod == 1)
        {
            string path = MainMenu.modelPath;
            model = new OBJLoader().Load(path);
            model.transform.SetParent(transform);
        }

        //Spawns a runtime-loaded model with its materials
        if (MainMenu.loadingMethod == 2)
        {
            
            string modelPath = MainMenu.modelPath;
            string materialPath = MainMenu.materialPath;
            if(materialPath.Equals("null"))
                model = new OBJLoader().Load(modelPath);
            else
                model = new OBJLoader().Load(modelPath, materialPath);
            model.transform.SetParent(transform);
        }
    }
}
