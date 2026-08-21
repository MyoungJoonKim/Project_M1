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
    [SerializeField] private Vector3 position = new Vector3(270, 500, 0);

    private MenuToggleUI menuToggleUI;

    private void Start()
    {
        menuToggleUI = GetComponentInParent<MenuToggleUI>();

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
        background.transform.position = position;

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
    }

    private IEnumerator TextFadeOutEffect()
    {
        text.gameObject.SetActive(true);
        text.transform.position = position;
        text.text = $"{menuToggleUI.LockOffLevel}·¹º§ ÀÌÈÄ ÄÁÅÙÃ÷ ÇØÁ¦";

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
    }


}
