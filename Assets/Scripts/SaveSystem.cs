using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;
using System.Linq;

public static class SaveSystem
{

    private static readonly string path = Application.persistentDataPath;
    private static readonly string settingsPath = path + "/settings.ini";
    private static readonly string modelsPath = path + "/models.list";
    private static readonly string materialsPath = path + "/materials.list";
    private static readonly string texturesPath = path + "/textures.list";
    public static void SaveSettings(Settings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(settingsPath, FileMode.Create);

        PlayerSettings data = new PlayerSettings(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerSettings LoadSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(settingsPath))
        {
            FileStream stream = new FileStream(settingsPath, FileMode.Open);
            PlayerSettings data = formatter.Deserialize(stream) as PlayerSettings;
            stream.Close();
            if(data != null)
                return data;
            else
                return new PlayerSettings();
        }
        else
        {
            FileStream stream = new FileStream(settingsPath, FileMode.Create);
            formatter.Serialize(stream, new PlayerSettings());
            stream.Close();
            return new PlayerSettings();
        }
    }

    public static void SaveModelPaths(string modelPath)
    {
        
        List<string> models = LoadModelsPaths();
        if (!CheckIfAlreadySaved(models, modelPath))
        {
            models.Add(modelPath);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(modelsPath, FileMode.Create);

            formatter.Serialize(stream, models);
            stream.Close();
        }
    }

    public static List<string> LoadModelsPaths()
    {
        if (File.Exists(modelsPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(modelsPath, FileMode.Open);
            if (stream.Length == 0)
            {
                stream.Close();
                return new List<string>();
            }


            List<string> models = formatter.Deserialize(stream) as List<string>;
            stream.Close();
            return models;
        }
        else
        {
            File.Create(modelsPath);
            return new List<string>();
        }
    }

    public static void RemoveModelPaths(string modelPath)
    {
        List<string> models = LoadModelsPaths();

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i].Equals(modelPath))
                models.RemoveAt(i);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(modelsPath, FileMode.Create);

        formatter.Serialize(stream, models);
        stream.Close();
    }

    public static void SaveMaterialsPaths(string materialPath)
    {

        List<string> materials = LoadMaterialsPaths();
        if (!CheckIfAlreadySaved(materials, materialPath))
        {
            materials.Add(materialPath);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(materialsPath, FileMode.Create);

            formatter.Serialize(stream, materials);
            stream.Close();
        }
    }

    public static List<string> LoadMaterialsPaths()
    {
        if (File.Exists(materialsPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(materialsPath, FileMode.Open);
            if (stream.Length == 0)
            {
                stream.Close();
                return new List<string>();
            }


            List<string> materials = formatter.Deserialize(stream) as List<string>;

            stream.Close();
            return materials;
        }
        else
        {
            File.Create(materialsPath);
            return new List<string>();
        }
    }

    public static void RemoveMaterialsPaths(string materialPath)
    {
        List<string> materials = LoadMaterialsPaths();

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i].Equals(materialPath))
                materials.RemoveAt(i);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(materialsPath, FileMode.Create);

        formatter.Serialize(stream, materials);
        stream.Close();
    }

    public static void NullifyMaterialPath(string materialPath)
    {
        List<string> materials = LoadMaterialsPaths();

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i].Equals(materialPath))
                materials[i] = "null";
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(materialsPath, FileMode.Create);

        formatter.Serialize(stream, materials);
        stream.Close();
    }

    public static void ChangeMaterialPath(string modelPath, string materialPath)
    {
        List<string> models = LoadModelsPaths();
        List<string> materials = LoadMaterialsPaths();

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i].Equals(modelPath) && materials[i].Equals("null"))
                materials[i] = materialPath;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(materialsPath, FileMode.Create);

        formatter.Serialize(stream, materials);
        stream.Close();
    }

    public static bool SaveTexturesPaths(List<string> texturePath)
    {

        List<List<string>> textures = LoadTexturesPaths();
        if (!CheckIfTexturesAlreadySaved(textures, texturePath))
        {
            textures.Add(texturePath);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(texturesPath, FileMode.Create);

            formatter.Serialize(stream, textures);
            stream.Close();
            return true;
        }
        return false;
    }

    public static List<List<string>> LoadTexturesPaths()
    {
        if (File.Exists(texturesPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(texturesPath, FileMode.Open);
            if (stream.Length == 0)
            {
                stream.Close();
                return new List<List<string>>();
            }

            List<List<string>> textures = formatter.Deserialize(stream) as List<List<string>>;

            stream.Close();
            return textures;
        }
        else
        {
            File.Create(texturesPath);
            return new List<List<string>>();
        }
    }

    private static bool CheckIfAlreadySaved(List<string> saved, string check)
    {
        for (int i = 0; i < saved.Count; i++)
        {
            if (saved[i].Equals(check))
                return true;
        }
        return false;
    }
    private static bool CheckIfTexturesAlreadySaved(List<List<string>> saved, List<string> check)
    {
        for (int i = 0; i < saved.Count; i++)
        {
            if (saved[i].SequenceEqual(check))
                return true;
        }
        return false;
    }

    public static void DeleteData()
    {
        if (File.Exists(modelsPath))
            File.Delete(modelsPath);
        if (File.Exists(materialsPath))
            File.Delete(materialsPath);
        if (File.Exists(texturesPath))
            File.Delete(texturesPath);
    }

    public static void ResetSettings()
    {
        if (File.Exists(settingsPath))
            File.Delete(settingsPath);
    }
}
