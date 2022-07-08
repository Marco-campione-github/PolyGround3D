using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    //For standard models
    public static int openedModelIndex;
    public List<string> models;
    public static UnityEngine.Object[] resources;
    public static bool isSettingsPanelOpen;

    //UI stuff
    public GameObject modelPanel;
    private PopUpSystem pop;

    //For models from files
    public static string modelPath;
    public static string materialPath;
    public static int loadingMethod;
    private static bool isFileBrowserOpen;
    private List<string> modelPathApp;
    private List<string> materialPathApp;
    private SceneLoader sceneLoader;

    private void Start()
    {
        isSettingsPanelOpen = false;
        isFileBrowserOpen = false;
        modelPathApp = new List<string>();
        materialPathApp = new List<string>();
        sceneLoader = gameObject.GetComponent<SceneLoader>();
        pop = gameObject.GetComponent<PopUpSystem>();
        resources = ResourceLoader.getGameObjects();

        for (int i = 0; i < resources.Length; i++)
        {
            models.Add(resources[i].name);
        }

        CreateModelPanels();
    }

    public void OpenModel()
    {
        if (!(isSettingsPanelOpen || isFileBrowserOpen))
        {
            GameObject gameObject = EventSystem.current.currentSelectedGameObject;
            int index = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
            openedModelIndex = index;
            loadingMethod = 0;
            //SceneManager.LoadScene(1);
            sceneLoader.LoadMain(); //Load the main scene
            // tells menu to freeze everything and slider 
        }
    }

    public static void OpenSettings(GameObject settingsPanel)
    {
        if (!isFileBrowserOpen)
            if (!settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(true);
                isSettingsPanelOpen = true;
            }
            else
            {
                settingsPanel.SetActive(false);
                isSettingsPanelOpen = false;
            }
    }

    private void CreateModelPanels()
    {
        //Get models paths from save system
        List<string> userModels = SaveSystem.LoadModelsPaths();

        //Get materials paths from save system
        List<string> userMaterials = SaveSystem.LoadMaterialsPaths();

        for (int i = 0; i < modelPanel.transform.childCount; i++)
            modelPanel.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);

        int counter = 0;
        for (int i = 0; i < models.Count; i++)
        {

            Transform obj = modelPanel.transform.GetChild(counter);

            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = ResourceLoader.getRenderByName(models[i]);
            obj.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OpenModel);
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(models[i][2].ToString().ToUpper() + models[i][3..]);
            obj.transform.GetChild(0).name = "UserModel" + i;

            counter++;
        }

        for (int i = 0; i < userModels.Count; i++)
        {
            Transform obj = modelPanel.transform.GetChild(counter);

            obj.transform.GetChild(0).gameObject.SetActive(true);
            obj.transform.GetChild(0).GetComponent<Image>().sprite = ResourceLoader.getRenderByName("user_model");
            obj.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
            modelPathApp.Add(userModels[i]);
            materialPathApp.Add(userMaterials[i]);
            obj.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(DelegateListener);
            obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(
                Path.GetFileName(userModels[i]));
            obj.transform.GetChild(0).name = "" + i;

            counter++;
        }
    }

    public void OpenModelFromFile(string path)
    {

        loadingMethod = 1;
        modelPath = path;
        //SceneManager.LoadScene(1);
        sceneLoader.LoadMain(); //Load the main scene
        // tells menu to freeze everything and slider 
    }

    public void OpenModelAndMaterialFromFile(string loadedModelPath, string loadedMaterialPath)
    {

        loadingMethod = 2;
        modelPath = loadedModelPath;
        materialPath = loadedMaterialPath;
        //SceneManager.LoadScene(1);
        sceneLoader.LoadMain(); //Load the main scene
        // tells menu to freeze everything and slider 

    }

    public void SetFileBrowserOpen(bool state)
    {
        isFileBrowserOpen = state;
    }


    // listener di appoggio per poter passare un parametro nell' onclick del button
    private void DelegateListener()
    {
        int index = Int32.Parse(EventSystem.current.currentSelectedGameObject.name);

        if (File.Exists(modelPathApp[index]))
        {
            if (File.Exists(materialPathApp[index]) || materialPathApp[index].Equals("null"))
                OpenModelAndMaterialFromFile(modelPathApp[index], materialPathApp[index]);
            else
            {
                SaveSystem.NullifyMaterialPath(materialPathApp[index]); //change path -> "null"
                OpenModelFromFile(modelPathApp[index]);
            }
        }
        else
        {
            SaveSystem.RemoveModelPaths(modelPathApp[index]);
            SaveSystem.RemoveMaterialsPaths(materialPathApp[index]);
            pop.OpenPopUp("Il file del modello non esiste piú!");
        }
    }

    //Button on settings to delete models and materials
    public void DeleteCache()
    {
        SaveSystem.DeleteData();
        pop.OpenPopUp("Modelli e materiali eliminati");
        RefreshModelPanels();
    }

    private void RefreshModelPanels()
    {
        CreateModelPanels();
    }

    public void InfoPopUp()
    {
        pop.OpenPopUp("ISTRUZIONI USO:\n\n- Per caricare un modello" +
            " con il suo materiale selezionare il file .obj e" +
            " il file .mtl insieme\n\n- Per caricare un set di texture" +
            " selezionare SOLO la texture colore e le texture secondarie" +
            " verranno caricate in automatico;\nDovranno chiamarsi come la texture colore:" +
            "\n+ \"_nor\" per la bump map," +
            "\n+ \"_met\" per la metal map," +
            "\n+ \"_hei\" per la height map," +
            "\n+ \"_occ\" per la occlusion map," +
            "\n+ \"_emi\" per la emission map");
    }
}

