using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventTextUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameObject warningEventUI;
    [SerializeField] private Image warningImage;

    [Header("Texts")]
    [SerializeField] private TMP_Text warningTextUI;
    [SerializeField] private TMP_Text timeText;

    private Coroutine warningTextCoroutine;
    private Coroutine timeTextCoroutine;

    private void Start()
    {
        warningEventUI.SetActive(false);
        warningImage.gameObject.SetActive(false);
        warningTextUI.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

    }

    public void Open()
    {
        if (warningEventUI != null) 
            warningEventUI.SetActive(true);

        if (warningImage != null)
            warningImage.gameObject.SetActive(true);

        if (warningTextUI != null)
        {
            if (warningTextCoroutine != null)
                StopCoroutine(warningTextCoroutine);

            warningTextCoroutine = StartCoroutine(WarningEventText());
        }

        if (timeText != null)
            timeText.gameObject.SetActive(true);

        if (timeText != null)
        {
            if (timeTextCoroutine != null)
                StopCoroutine(timeTextCoroutine);

            timeTextCoroutine = StartCoroutine(UpdateTimeUI());
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

    private IEnumerator UpdateTimeUI()
    {
        while (!Shared.eventManager.EventFail)
        {
            if (Shared.battleManager == null)
            {
                yield return null;
                continue;
            }

            if (timeText == null)
                yield break;

            float time = Shared.eventManager.Timer;

            int second = Mathf.FloorToInt(time % 60f);

            timeText.text = $"{second:00}";

            yield return null;
        }
        timeText.gameObject.SetActive(false);
        warningImage.gameObject.SetActive(false);

        timeTextCoroutine = null;
    }

}
