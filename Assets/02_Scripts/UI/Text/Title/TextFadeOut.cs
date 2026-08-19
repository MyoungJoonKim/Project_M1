using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeOut : MonoBehaviour
{
    [Header("Menu Lock UI")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text text;
    [SerializeField] private float fadeTime = 3f;

    private void Start()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void Open()
    {
        StartCoroutine(BackgroundFadeOutEffect());
        StartCoroutine(TextFadeOutEffect());
    }

    private IEnumerator BackgroundFadeOutEffect()
    {
        background.gameObject.SetActive(true);

        Color color = background.color;
        color.a = 150f;
        background.color = color;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(150f, 0f, timer / fadeTime);
            background.color = color;

            yield return null;
        }
        color.a = 0f;
        background.color = color;
        background.gameObject.SetActive(false);
    }

    private IEnumerator TextFadeOutEffect()
    {
        text.gameObject.SetActive(true);

        Color color = text.color;
        color.a = 255f;
        text.color = color;
        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(255f, 0f, timer / fadeTime);
            text.color = color;

            yield return null;
        }
        color.a = 0f;
        text.color = color;
        text.gameObject.SetActive(false);
    }


}
