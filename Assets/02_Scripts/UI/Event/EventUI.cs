using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class EventUI : MonoBehaviour
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
            yield return new WaitForSeconds(1f);
            warningTextUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            warningTextUI.gameObject.SetActive(false);
        }
        warningTextCoroutine = null;
    }




}
