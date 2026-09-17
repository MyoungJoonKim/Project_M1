using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CloseUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panelParent;
    [SerializeField] private CanvasGroup panel;

    [Header("UI Effect")]
    [SerializeField] private float speed = 7f;

    public void OnClickCloseUI()
    {
        SoundManager.Instance.PlayButtonClick();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        panel.alpha = 1f;
        panel.transform.localScale = Vector3.one;

        while (panel.alpha > 0f)
        {
            panel.alpha -= speed * Time.unscaledDeltaTime;
            panel.transform.localScale -= Vector3.one * speed * Time.unscaledDeltaTime;
            yield return null;
        }

        panel.alpha = 0f;
        panel.transform.localScale = Vector3.zero;

        if (panelParent != null)
            panelParent.SetActive(false);
    }
}
