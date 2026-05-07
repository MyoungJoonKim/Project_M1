using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TMP_TextBlink : MonoBehaviour
{
    private TextMeshProUGUI TouchTheScreen;
    public bool textBlink = true;

    [Header("색상")]
    public Color whiteColor = new Color(1f, 1f, 1f, 1f);
    public Color grayColor = new Color(1f, 1f, 1f, 0.35f);

    [Header("속도")]
    public float speed = 2f;

    private void Awake()
    {
        if (Shared.TMP_TextBlink == null)
            Shared.TMP_TextBlink = this;

        TouchTheScreen = GetComponent<TextMeshProUGUI>();
        StartCoroutine(TextBlinkUpdate());
    }

    IEnumerator TextBlinkUpdate()
    {
        while (textBlink)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            TouchTheScreen.color = Color.Lerp(grayColor, whiteColor, t);
            yield return null;
        }
    }


}
