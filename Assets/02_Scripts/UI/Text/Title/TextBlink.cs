using System.Collections;
using TMPro;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    private TextMeshProUGUI TouchTheScreen;
    public bool isTextBlink = true;

    [Header("Colors")]
    [SerializeField] private Color whiteColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color grayColor = new Color(1f, 1f, 1f, 0.35f);

    [Header("value")]
    [SerializeField] private float speed = 2f;

    private void Awake()
    {
        TouchTheScreen = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TextBlinkEffect());
    }

    private IEnumerator TextBlinkEffect()
    {
        while (isTextBlink)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            TouchTheScreen.color = Color.Lerp(grayColor, whiteColor, t);
            yield return null;
        }
    }


}
