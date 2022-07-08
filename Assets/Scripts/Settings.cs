using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [HideInInspector] public float touchSensitivity;
    [HideInInspector] public float zoomSensitivity;

    public Slider touchSlider;
    public Slider zoomSlider;
    private void Start()
    {
        LoadSettings();
        zoomSlider.value = zoomSensitivity;
        touchSlider.value = touchSensitivity;
    }

    public void SaveSettings(GameObject settingsPanel)
    {
        touchSensitivity = touchSlider.value;
        zoomSensitivity = zoomSlider.value;
        SaveSystem.SaveSettings(this);
        MainMenu.OpenSettings(settingsPanel);
    }

    public void LoadSettings()
    {
        PlayerSettings data = SaveSystem.LoadSettings();
        touchSensitivity = data.touchSensitivity;
        zoomSensitivity = data.zoomSensitivity;
    }
}
