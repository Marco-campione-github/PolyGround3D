using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntensityController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textIntensity;
    // Start is called before the first frame update
    
    public void OnChangeIntensity(Light light)
    {
        light.GetComponent<Light>().intensity = slider.value;

        textIntensity.text = Mathf.RoundToInt(slider.value * 100) / 2 + "%";
    }
}
