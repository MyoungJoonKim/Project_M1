using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panelParent;
    [SerializeField] private CanvasGroup panel;

    [Header("UI Effect")]
    [SerializeField] private float speed = 10f;

    public void OnClickOpenUI()
    {
        SoundManager.Instance.PlayButtonClick();

        if (panelParent != null)
            panelParent.SetActive(true);

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        if (panelParent != null)
            panelParent.SetActive(true);

        panel.alpha = 0f;
        panel.transform.localScale = Vector3.zero;

        while (panel.alpha < 1f)
        {
            panel.alpha += speed * Time.unscaledDeltaTime;
            panel.transform.localScale += Vector3.one * speed * Time.unscaledDeltaTime;
            yield return null;
        }

        panel.alpha = 1f;
        panel.transform.localScale = Vector3.one;

        
    }
}
