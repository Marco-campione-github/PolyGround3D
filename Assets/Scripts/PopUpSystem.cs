using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    private void Start()
    {
        popUpBox.SetActive(false);
    }
    public void OpenPopUp(string text)
    {
        popUpText.text = text;
        popUpBox.SetActive(true);
        animator.SetTrigger("pop");
    }

    public void SetActiveFalseAfterTime()
    {
        StartCoroutine(SetActiveFalseAfterTimeCoroutine(1f));
    }

    IEnumerator SetActiveFalseAfterTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        popUpBox.SetActive(false);
    }
}
