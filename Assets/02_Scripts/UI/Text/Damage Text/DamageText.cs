using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private RectTransform textTransform;

    [Header("Text Effect")]
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float popScale = 1.4f;
    [SerializeField] private float moveDistance = 3f;

    private DamageTextManager manager;
    private Sequence sequence;

    public void SetManager(DamageTextManager manager)
    {
        this.manager = manager;
    }

    public void SetUp(float damage)
    {
        if (sequence != null)
        {
            sequence.Kill();
            sequence = null;
        }

        damageText.DOKill();
        textTransform.DOKill();

        damageText.text = Mathf.CeilToInt(damage).ToString("F0");

        damageText.alpha = 1f;
        textTransform.localScale = Vector3.one * 0.4f;

        Vector2 startPosition = textTransform.anchoredPosition;

        sequence = DOTween.Sequence();

        sequence.Append(textTransform.DOScale(popScale, 0.12f).SetEase(Ease.OutBack));

        sequence.Append(textTransform.DOScale(1f, 0.12f).SetEase(Ease.OutQuad));

        sequence.Insert(0f, textTransform.DOAnchorPosY(startPosition.y + moveDistance, duration).SetEase(Ease.OutQuad));

        sequence.Insert(duration * 0.55f, damageText.DOFade(0f, duration * 0.45f));

        sequence.OnComplete(() => 
        { 
            manager.Release(this); 
        });

        sequence.SetUpdate(true);

    }

    public void OnDisable()
    {
        if (sequence != null)
        {
            sequence.Kill();
            sequence = null;
        }
    }

}
