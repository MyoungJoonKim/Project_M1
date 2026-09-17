using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using DG.Tweening;

public class SkillSlotUI : MonoBehaviour
{
    [Header("Skill Info")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillInfoText;

    [Header("Skill Level Star")]
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite onStar;
    [SerializeField] private Sprite offStar;

    [Header("Skill Card")]
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private CanvasGroup panelGroup;


    public void SetSlot(ActiveSkillData data, int level)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        if (icon != null)
            icon.sprite = data.icon;

        if (skillNameText != null)
            skillNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.skillName);

        if (skillInfoText != null)
            skillInfoText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.skillInfo);

        UpdateStars(level);
    }

    public void SetSlot(PassiveSkillData data, int level)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        if (icon != null)
            icon.sprite = data.icon;

        if (skillNameText != null)
            skillNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.passiveSkillName);

        if (skillInfoText != null)
            skillInfoText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.skillInfo);

        UpdateStars(level);
    }

    public void ClearSlot()
    {
        gameObject.SetActive(true);

        if (skillNameText != null)
            skillNameText.text = "";

        if (skillInfoText != null)
            skillInfoText.text = "";

        UpdateStars(0);
    }


    // 스킬 현재 레벨 표시 함수
    private void UpdateStars(int level)
    {
        if (stars == null) 
            return;

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] == null)
                continue;

            if (i < level)
                stars[i].sprite = onStar;
            else
                stars[i].sprite = offStar;
        }
    }

    public Tween SkillCardOpen(float delay = 0f)
    {
        cardPanel.transform.DOKill();
        panelGroup.DOKill();

        cardPanel.transform.localScale = Vector3.one * 0.4f;
        cardPanel.transform.localRotation = Quaternion.Euler(0f, 90f, -10f);
        panelGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(delay);

        sequence.Append(cardPanel.transform.DORotate(Vector3.zero, 0.25f).SetEase(Ease.OutQuad));

        sequence.Join(cardPanel.transform.DOScale(1.15f, 0.25f).SetEase(Ease.OutBack));

        sequence.Join(panelGroup.DOFade(1f, 0.15f));

        sequence.Append(cardPanel.transform.DOScale(1f, 0.08f).SetEase(Ease.OutQuad));

        sequence.SetUpdate(true);
        return sequence;
    }
}
