using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            skillNameText.text = data.skillName;

        if (skillInfoText != null)
            skillInfoText.text = data.skillInfo;

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
            skillNameText.text = data.passiveSkillName;

        if (skillInfoText != null)
            skillInfoText.text = data.skillInfo;

        UpdateStars(level);
    }

    public void ClearSlot()
    {
        gameObject.SetActive(true);

        if (icon != null)
        {
            icon.sprite = null;
        }

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
}
