using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeOut : MonoBehaviour
{
    [Header("Menu Lock UI")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeTime = 3f;

    private Coroutine backgroundFadeOutCoroutine;
    private Coroutine textFadeOutCoroutine;

    private void Start()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void Open(int lockOffLevel)
    {
        if (backgroundFadeOutCoroutine != null || textFadeOutCoroutine != null)
        {
            StopCoroutine(backgroundFadeOutCoroutine);
            StopCoroutine(textFadeOutCoroutine);
        }
        backgroundFadeOutCoroutine = StartCoroutine(BackgroundFadeOutEffect());
        textFadeOutCoroutine = StartCoroutine(TextFadeOutEffect(lockOffLevel));
    }

    private IEnumerator BackgroundFadeOutEffect()
    {
        background.gameObject.SetActive(true);

        Color color = background.color;
        color.a = 1f;
        background.color = color;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            background.color = color;

            yield return null;
        }
        color.a = 0f;
        background.color = color;
        background.gameObject.SetActive(false);

        backgroundFadeOutCoroutine = null;
    }

    private IEnumerator TextFadeOutEffect(int lockOffLevel)
    {
        text.gameObject.SetActive(true);
        text.text = $"{lockOffLevel}·¹º§ ÀÌÈÄ ÄÁÅÙÃ÷ ÇØÁ¦";

        Color color = text.color;
        color.a = 1f;
        text.color = color;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / fadeTime);
            text.color = color;

            yield return null;
        }
        color.a = 0f;
        text.color = color;
        text.gameObject.SetActive(false);

        textFadeOutCoroutine = null;
    }


}
