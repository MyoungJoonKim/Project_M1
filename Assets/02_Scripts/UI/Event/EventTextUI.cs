using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventTextUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameObject warningEventUI;
    [SerializeField] private TMP_Text warningTextUI;


    private Coroutine warningTextCoroutine;

    private void Start()
    {
        warningEventUI.SetActive(false);
        warningTextUI.gameObject.SetActive(false);
    }

    public void Open()
    {
        if (warningEventUI != null) 
            warningEventUI.SetActive(true);

        if (warningTextUI != null)
        {
            if (warningTextCoroutine != null)
                StopCoroutine(warningTextCoroutine);

            warningTextCoroutine = StartCoroutine(WarningEventText());
        }    
    }

    private IEnumerator WarningEventText()
    {
        if (warningEventUI.activeSelf)
        {
            yield return new WaitForSeconds(1.5f);
            warningTextUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            warningTextUI.gameObject.SetActive(false);
        }
        warningTextCoroutine = null;
    }

}
