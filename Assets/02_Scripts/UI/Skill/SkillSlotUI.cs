using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite onStar;
    [SerializeField] private Sprite offStar;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillInfoText;
    [SerializeField] private Button button;

    private SkillData skillData;
    private SkillSelectUI skillSelectUI;

    public void SetSlot(SkillData data, SkillSelectUI uI)
    {
        skillData = data;
        skillSelectUI = uI;

        if (icon != null)
            icon.sprite = skillData.icon;

        if (skillNameText != null)
            skillNameText.text = skillData.skillName;

        if (skillInfoText != null)
            skillInfoText.text = skillData.skillInfo;

        int level = skillSelectUI.GetSkillLevel(skillData);

        UpdateStars(level);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSlot);
    }

    // 스킬 현재 레벨 표시 함수
    private void UpdateStars(int level)
    {
        if (stars == null || stars.Length == 0) 
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

    private void OnClickSlot()
    {
        if (skillSelectUI == null || skillData == null) 
            return;

        skillSelectUI.SelectSkill(skillData);
    }
}
