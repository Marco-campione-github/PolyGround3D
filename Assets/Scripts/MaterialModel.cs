using Dummiesman;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MaterialModel : MonoBehaviour
{
    GameObject model;
    List<Material[]> originalMaterials;
    Object[] materials;
    public GameObject scrollViewController;
    public GameObject UIManager;
    private int currentShader;

    void Start()
    {
        model = transform.GetChild(3).gameObject; //0, 1, 2 sono gli assi xyz
        materials = ResourceLoader.getMaterials();
        currentShader = 0;
        LoadTexturesIntoMaterials();
        originalMaterials = new List<Material[]>();
        StoreOriginalMaterials();
    }

    public void ChangeMaterial(int index)
    {
        //caso in cui bisogna usare un materiale precaricato (o aggiunto a posteriori)
        if (index > 0)
        {
            Material currMat = (Material)materials[index - 1];
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] tmp = new Material[renderers[i].materials.Length];
                for (int j = 0; j < tmp.Length; j++)
                    tmp[j] = currMat;
                renderers[i].materials = tmp;
            }
            currentShader = 0;
            UIManager.GetComponent<UIManager>().ChangeTexturePreviews(
            currMat.GetTexture("_MainTex") as Texture2D,
            currMat.GetTexture("_BumpMap") as Texture2D,
            currMat.GetTexture("_MetallicGlossMap") as Texture2D,
            currMat.GetTexture("_ParallaxMap") as Texture2D,
            currMat.GetTexture("_OcclusionMap") as Texture2D,
            currMat.GetTexture("_EmissionMap") as Texture2D);
        }

        //caso in cui bisogna usare il materiale originale
        if(index == 0)
        {
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] tmp = new Material[renderers[i].materials.Length];
                for (int j = 0; j < tmp.Length; j++)
                    tmp[j] = originalMaterials[i][j];
                renderers[i].materials = tmp;
            }
            currentShader = 0;
        }

        //caso in cui si carica il materiale Wireframe/ForceField o Plasma
        if (index < 0)
        {
            Material mat = null;
            if (index == -1)
                mat = ResourceLoader.getOtherMaterialsByName("Wireframe");
            if (index == -2)
                mat = ResourceLoader.getOtherMaterialsByName("ForceField");
            if (index == -3)
                mat = ResourceLoader.getOtherMaterialsByName("Plasma");
            
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] tmp = new Material[renderers[i].materials.Length];
                for (int j = 0; j < tmp.Length; j++)
                    tmp[j] = mat;
                renderers[i].materials = tmp;
            }
            currentShader = index;
        }
    }

    public int GetCurrentShader()
    {
        return currentShader;
    }

    internal int getCurrentShader()
    {
        throw new System.NotImplementedException();
    }

    private void StoreOriginalMaterials()
    {
        MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials.Add(renderers[i].materials);
        }
    }

    public void AddMaterial(Material material, bool change)
    {
        materials = materials.Concat(new Object[] { material }).ToArray();
        scrollViewController.GetComponent<TextureScrollDataSource>().AddTextureFromMaterial(material);
        if (change)
        {
            ChangeMaterial(materials.Length);
            UIManager.GetComponent<UIManager>().SetCurrentTexture(materials.Length);
        }
    
    }

    private bool IsModelComposite()
    {
        if (model.GetComponent<MeshRenderer>()) return false;
        return true;
    }

    public void SetRoughness(float value)
    {
        float trueValue = 1 - value; //Perche roughness è il contrario di smoothness
        MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                renderers[i].materials[j].SetFloat("_Glossiness", trueValue);
            }
        }
    }

    public float GetRoughness()
    {
        MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();
        return 1 - renderers[0].material.GetFloat("_Glossiness"); //Smoothness --> Roughness
    }

    private void LoadTexturesIntoMaterials()
    {
        List<List<string>> texturesPaths = SaveSystem.LoadTexturesPaths();
        
        for (int i = 0; i < texturesPaths.Count; i++)
        {
            List<Texture2D> textures = new();
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][0]));
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][1]));
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][2]));
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][3]));
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][4]));
            textures.Add(ImageLoader.LoadTexture(texturesPaths[i][5]));
            AddMaterial(GenerateMaterial(textures), false);
        } 
    }

    //Only apply the material but not add to materials (and not add to UI)
    public void ShowMaterial(Material material)
    {
        if (!IsModelComposite())
        {
            Material[] tmp = new Material[model.GetComponent<MeshRenderer>().materials.Length];
            for (int i = 0; i < tmp.Length; i++)
                tmp[i] = material;
            model.GetComponent<MeshRenderer>().materials = tmp;
        }
        else
            for (int i = 0; i < model.transform.childCount; i++)
            {
                Material[] tmp = new Material[model.transform.GetChild(i).GetComponent<MeshRenderer>().materials.Length];
                for (int j = 0; j < tmp.Length; j++)
                    tmp[j] = material;
                model.transform.GetChild(i).GetComponent<MeshRenderer>().materials = tmp;
            }
        UIManager.GetComponent<UIManager>().SetCurrentTexture(1000);
        UIManager.GetComponent<UIManager>().ChangeTexturePreviews(
            material.GetTexture("_MainTex") as Texture2D,
            material.GetTexture("_BumpMap") as Texture2D,
            material.GetTexture("_MetallicGlossMap") as Texture2D,
            material.GetTexture("_ParallaxMap") as Texture2D,
            material.GetTexture("_OcclusionMap") as Texture2D,
            material.GetTexture("_EmissionMap") as Texture2D);

    }

    public static Material GenerateMaterial(List<Texture2D> textures)
    {
        Material material = new(Shader.Find("Standard"));
        if (textures[0] != null)
            material.SetTexture("_MainTex", textures[0]);
        if(textures[1] != null)
        {
            material.EnableKeyword("_NORMALMAP");
            material.SetTexture("_BumpMap", textures[1]);
        }
        if(textures[2] != null)
        {
            material.EnableKeyword("_METALLICGLOSSMAP");
            material.SetTexture("_MetallicGlossMap", textures[2]);
        }
        if(textures[3] != null)
        {
            //material.EnableKeyword();
            material.SetTexture("_ParallaxMap", textures[3]);
        }
        if(textures[4] != null)
        {
            //material.EnableKeyword();
            material.SetTexture("_OcclusionMap", textures[4]);
        }
        if(textures[5] != null)
        {
            material.EnableKeyword("_EMISSION");
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            material.SetColor("_EmissionColor", Color.white); //emission color selectable?
            material.SetTexture("_EmissionMap", textures[5]);
        }
        return material;
    }

        // _"MainTex": color +
        // "_BumpMap": normal +
        // "_MetallicGlossMap": metallic +
        // "_ParallaxMap": height +
        // "_OcclusionMap": occlusion +
        // "_EmissionMap": emission
}
