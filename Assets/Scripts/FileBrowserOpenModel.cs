using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.SceneManagement;
using Dummiesman;
using UnityEngine.UI;
using System.Collections.Generic;

public class FileBrowserOpenModel : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject model;
    public GameObject ui;
    public PopUpSystem pop;
    private bool saved;
    private readonly string[] textureExtensions = { ".bmp", ".tga", ".jpg", ".png", ".dds", ".crn" };

    public void OpenFileBrowserForModels()
    {
        if (!MainMenu.isSettingsPanelOpen)
        {
            eventSystem.SetActive(false);
            SetModelFilters();
            ui.GetComponent<MainMenu>().SetFileBrowserOpen(true);
            StartCoroutine(ShowLoadDialogCoroutineForModels());
        }
    }

    public void OpenFileBrowserForImages()
    {
        eventSystem.SetActive(false);
        SetImageFilters();
        ui.GetComponent<UIManager>().SetFileBrowserOpen(true);
        StartCoroutine(ShowLoadDialogCoroutineForImages());
    }

    IEnumerator ShowLoadDialogCoroutineForModels()
    {

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Model and material", "Load");
        if (FileBrowser.Success)
        {
            if (FileBrowser.Result.Length == 0)
                pop.OpenPopUp("Selezionare almeno un file!");
            //Caso in cui si carica solo il modello
            if (FileBrowser.Result.Length == 1)
            {
                if (FileBrowser.Result[0].Contains(" "))
                    pop.OpenPopUp("Il percorso contiene spazi !");
                else
                    if (CheckFileExtension(FileBrowser.Result[0], new string[] { ".obj" }))
                {
                    SaveSystem.SaveModelPaths(FileBrowser.Result[0]);
                    SaveSystem.SaveMaterialsPaths("null");
                    ui.GetComponent<MainMenu>().OpenModelFromFile(FileBrowser.Result[0]);
                }
                else
                    pop.OpenPopUp("Estensione non corretta !");
            }
            //Caso in cui si carica il modello e un materiale
            if (FileBrowser.Result.Length == 2)
            {
                if (FileBrowser.Result[0].Contains(" ") || FileBrowser.Result[1].Contains(" "))
                    pop.OpenPopUp("Il percorso contiene spazi !");
                else
                {
                    if (CheckFileExtension(FileBrowser.Result[0], new string[] { ".obj" }) &&
                    CheckFileExtension(FileBrowser.Result[1], new string[] { ".mtl" }))
                    {
                        SaveSystem.ChangeMaterialPath(FileBrowser.Result[0], FileBrowser.Result[1]);
                        SaveSystem.SaveModelPaths(FileBrowser.Result[0]);
                        SaveSystem.SaveMaterialsPaths(FileBrowser.Result[1]);
                        ui.GetComponent<MainMenu>().OpenModelAndMaterialFromFile(
                                FileBrowser.Result[0],
                                FileBrowser.Result[1]);
                    }
                    else
                        if (CheckFileExtension(FileBrowser.Result[1], new string[] { ".obj" }) &&
                        CheckFileExtension(FileBrowser.Result[0], new string[] { ".mtl" }))
                    {
                        SaveSystem.ChangeMaterialPath(FileBrowser.Result[1], FileBrowser.Result[0]);
                        SaveSystem.SaveModelPaths(FileBrowser.Result[1]);
                        SaveSystem.SaveMaterialsPaths(FileBrowser.Result[0]);
                        ui.GetComponent<MainMenu>().OpenModelAndMaterialFromFile(
                            FileBrowser.Result[1],
                            FileBrowser.Result[0]);
                    }
                    else
                        pop.OpenPopUp("Estensione non corretta !");
                }
            }
            if (FileBrowser.Result.Length > 2)
                pop.OpenPopUp("Hai selezionato troppi file!");

        }

        eventSystem.SetActive(true);
        ui.GetComponent<MainMenu>().SetFileBrowserOpen(false);
    }

    //  esempio:
    //	colore: "texture1.xxx"
    //	normal: "texture1_nor.xxx"
    //  metallic: "texture1_met.xxx"
    //  height: "texture1_hei.xxx"
    //  occlusion: "texture1_occ.xxx"
    //  emission: "texture1_emi.xxx"
    //	NOTA: tutte le texure devono essere nella stessa cartella
    IEnumerator ShowLoadDialogCoroutineForImages()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Textures", "Load");

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                if (FileBrowser.Result[i].Contains(" "))
                    pop.OpenPopUp("Il percorso contiene spazi !");
                else
                    if (CheckFileExtension(FileBrowser.Result[i], textureExtensions)) //TODO: CHANGE EXTENSIONS
                {

                    List<Texture2D> textures = GetTextures(FileBrowser.Result[i]);
                    Material material = MaterialModel.GenerateMaterial(textures);

                    if (material != null)
                        if (saved)
                            //Add material to current loaded materials
                            model.GetComponent<MaterialModel>().AddMaterial(material, true);
                        else
                            //Just show the material that is alreasy saved!
                            model.GetComponent<MaterialModel>().ShowMaterial(material);
                }
                else
                {
                    pop.OpenPopUp("Estensione non corretta !");
                }
            }


        }

        eventSystem.SetActive(true);
        ui.GetComponent<UIManager>().SetFileBrowserOpen(false);
    }

    private void SetImageFilters()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", textureExtensions));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }

    private void SetModelFilters()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("3D Models", ".obj", "mtl"));
        FileBrowser.SetDefaultFilter(".obj");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }
    private bool CheckFileExtension(string path, string[] expectedExtensions)
    {
        string extension = Path.GetExtension(path);
        for (int i = 0; i < expectedExtensions.Length; i++)
        {
            if (extension.ToLower().Equals(expectedExtensions[i]))
                return true;
        }
        return false;
    }

    private bool CheckModelExistsWithoutMaterial(string modelPath)
    {
        List<string> models = SaveSystem.LoadModelsPaths();
        List<string> materials = SaveSystem.LoadMaterialsPaths();

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i].Equals(modelPath) && materials[i].Equals("null"))
                return true;
        }
        return false;
    }

    private List<Texture2D> GetTextures(string colorPath)
    {
        List<Texture2D> textures = new();
        List<string> paths = new();
        paths.Add(colorPath);
        paths.Add(Path.ChangeExtension(colorPath, null) +
                            "_nor" + GetTextureExtension(colorPath, "_nor"));
        paths.Add(Path.ChangeExtension(colorPath, null) +
                            "_met" + GetTextureExtension(colorPath, "_met"));
        paths.Add(Path.ChangeExtension(colorPath, null) +
                            "_hei" + GetTextureExtension(colorPath, "_hei"));
        paths.Add(Path.ChangeExtension(colorPath, null) +
                            "_occ" + GetTextureExtension(colorPath, "_occ"));
        paths.Add(Path.ChangeExtension(colorPath, null) +
                            "_emi" + GetTextureExtension(colorPath, "_emi"));
        textures.Add(ImageLoader.LoadTexture(paths[0]));
        textures.Add(ImageLoader.LoadTexture(paths[1]));
        textures.Add(ImageLoader.LoadTexture(paths[2]));
        textures.Add(ImageLoader.LoadTexture(paths[3]));
        textures.Add(ImageLoader.LoadTexture(paths[4]));
        textures.Add(ImageLoader.LoadTexture(paths[5]));
        //Save the textures to FileSystem
        saved = SaveSystem.SaveTexturesPaths(paths);
        return textures;
    }

    private string GetTextureExtension(string colorPath, string textureType)
    {
        for(int i = 0; i < textureExtensions.Length; i++)
        {
            if (File.Exists(Path.ChangeExtension(colorPath, null) + textureType + textureExtensions[i]))
                return textureExtensions[i];
        }
        return ".png"; //default
    }
}