using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;

    public void LoadMain()
    {
        StartCoroutine(LoadAsync());
        
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}