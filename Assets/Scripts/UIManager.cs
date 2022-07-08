using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject lightPanel;
    [SerializeField] private GameObject texturePanel;

    private int currentTexture;

    public GameObject centerPoint;
    private static bool isFileBrowserOpen;
    public static bool arePanelsOpen;

    public GameObject xAxisIndicator;
    public GameObject yAxisIndicator;
    public GameObject zAxisIndicator;

    public GameObject colorTexturePreview;
    public GameObject normalTexturePreview;
    public GameObject metallicTexturePreview;
    public GameObject heightTexturePreview;
    public GameObject occlusionTexturePreview;
    public GameObject emissionTexturePreview;

    public GameObject colorTexturePreviewLabel;
    public GameObject normalTexturePreviewLabel;
    public GameObject metallicTexturePreviewLabel;
    public GameObject heightTexturePreviewLabel;
    public GameObject occlusionTexturePreviewLabel;
    public GameObject emissionTexturePreviewLabel;

    private void Start()
    {
        arePanelsOpen = false;
        isFileBrowserOpen = false;
        currentTexture = 0;

    }

    private void Update()
    {
        /* change shader button */

        if (currentTexture <= 0)
        {
            colorTexturePreview.SetActive(false);
            normalTexturePreview.SetActive(false);
            metallicTexturePreview.SetActive(false);
            heightTexturePreview.SetActive(false);
            occlusionTexturePreview.SetActive(false);
            emissionTexturePreview.SetActive(false);

            colorTexturePreviewLabel.SetActive(false);
            normalTexturePreviewLabel.SetActive(false);
            metallicTexturePreviewLabel.SetActive(false);
            heightTexturePreviewLabel.SetActive(false);
            occlusionTexturePreviewLabel.SetActive(false);
            emissionTexturePreviewLabel.SetActive(false);

        }
        else
        {
            colorTexturePreview.SetActive(true);
            normalTexturePreview.SetActive(true);
            metallicTexturePreview.SetActive(true);
            heightTexturePreview.SetActive(true);
            occlusionTexturePreview.SetActive(true);
            emissionTexturePreview.SetActive(true);

            colorTexturePreviewLabel.SetActive(true);
            normalTexturePreviewLabel.SetActive(true);
            metallicTexturePreviewLabel.SetActive(true);
            heightTexturePreviewLabel.SetActive(true);
            occlusionTexturePreviewLabel.SetActive(true);
            emissionTexturePreviewLabel.SetActive(true);
        }
    }

    //Opens and closes light panel
    public void OpenLightPanel()
    {
        if (!lightPanel.activeSelf && !texturePanel.activeSelf)
        {
            lightPanel.SetActive(true);
        }
        else
        {
            lightPanel.SetActive(false);
        }
        UpdatePanels();
    }

    public void OpenTexturePanel()
    {
        if (!texturePanel.activeSelf && !lightPanel.activeSelf)
        {
            texturePanel.SetActive(true);
        }
        else
        {
            texturePanel.SetActive(false);
        }
        UpdatePanels();
    }

    private void UpdatePanels()
    {
        if (lightPanel.activeSelf || isFileBrowserOpen || texturePanel.activeSelf)
            arePanelsOpen = true;
        else
            arePanelsOpen = false;
    }

    public void SetFileBrowserOpen(bool state)
    {
        isFileBrowserOpen = state;
        UpdatePanels();
    }

    //Back button to Menu
    public void OpenMenu()
    {
        SceneManager.LoadScene(0); // Load the menu scene
    }

    public void ResetLightRotation(Light directionalLight)
    {
        directionalLight.GetComponent<LightController>().ResetRotation();
    }

    public void SetCurrentTexture(int index)
    {
        currentTexture = index;
    }

    public void ChangeShader()
    {
        int current = centerPoint.GetComponent<MaterialModel>().GetCurrentShader();

        if (current == 0)
        {
            currentTexture = -1;
            WireframeMaterial();
        }

        if (current == -1)
        {
            currentTexture = -2;
            ForceFieldMaterial();
        }

        if (current == -2)
        {
            currentTexture = -3;
            PlasmaMaterial();
        }

        if (current == -3)
        {
            currentTexture = 0;
            OriginalMaterial();
        }
    }

    private void WireframeMaterial()
    {
        centerPoint.GetComponent<MaterialModel>().ChangeMaterial(-1);
    }

    private void ForceFieldMaterial()
    {
        centerPoint.GetComponent<MaterialModel>().ChangeMaterial(-2);
    }

    private void PlasmaMaterial()
    {
        centerPoint.GetComponent<MaterialModel>().ChangeMaterial(-3);
    }

    private void OriginalMaterial()
    {
        centerPoint.GetComponent<MaterialModel>().ChangeMaterial(0);
    }

    public void DisableAxisIndicators()
    {
        if (xAxisIndicator.activeSelf)
        {
            xAxisIndicator.SetActive(false);
            yAxisIndicator.SetActive(false);
            zAxisIndicator.SetActive(false);
        }
        else
        {
            xAxisIndicator.SetActive(true);
            yAxisIndicator.SetActive(true);
            zAxisIndicator.SetActive(true);
        }
    }

    public void ChangeTexturePreviews(
        Texture2D colorTexture,
        Texture2D normalTexture,
        Texture2D metallicTexture,
        Texture2D heightTexture,
        Texture2D occlusionTexture,
        Texture2D emissionTexture)
    {
        Sprite missing = ResourceLoader.getRenderByName("missing_texture");
        if (colorTexture != null)
            colorTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                colorTexture,
                new Rect(0.0f, 0.0f, colorTexture.width, colorTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            colorTexturePreview.GetComponent<Image>().sprite = missing;

        if (normalTexture != null)
            normalTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                normalTexture,
                new Rect(0.0f, 0.0f, normalTexture.width, normalTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            normalTexturePreview.GetComponent<Image>().sprite = missing;

        if (metallicTexture != null)
            metallicTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                metallicTexture,
                new Rect(0.0f, 0.0f, metallicTexture.width, metallicTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            metallicTexturePreview.GetComponent<Image>().sprite = missing;

        if (heightTexture != null)
            heightTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                heightTexture,
                new Rect(0.0f, 0.0f, heightTexture.width, heightTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            heightTexturePreview.GetComponent<Image>().sprite = missing;

        if (occlusionTexture != null)
            occlusionTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                occlusionTexture,
                new Rect(0.0f, 0.0f, occlusionTexture.width, occlusionTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            occlusionTexturePreview.GetComponent<Image>().sprite = missing;

        if (emissionTexture != null)
            emissionTexturePreview.GetComponent<Image>().sprite = Sprite.Create(
                emissionTexture,
                new Rect(0.0f, 0.0f, emissionTexture.width, emissionTexture.height),
                new Vector2(0.5f, 0.5f),
                100.0f);
        else
            emissionTexturePreview.GetComponent<Image>().sprite = missing;
    }
}
