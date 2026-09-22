using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class AbilityManager : MonoBehaviour
{
    [Header("Slot")]
    [SerializeField] private AbilitySlotUI[] slots;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Description Panels")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject descriptionClosePanel;
    [SerializeField] private TextFadeOut textFadeOut;

    [Header("Description Texts")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;

    [Header("Ability Button")]
    [SerializeField] private Button buyButton;


    private AbilitySlotUI selectedSlot;
    private OpenUI openUI;
    private CloseUI closeUI;

    private void Awake()
    {
        openUI = GetComponent<OpenUI>();
        closeUI = GetComponent<CloseUI>();
    }

    private void Start()
    {
        foreach (AbilitySlotUI slot in slots)
        {
            if (slot == null) 
                continue;

            slot.Init(this);
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(OnClickBuyAbility);
            buyButton.onClick.AddListener(OnClickBuyAbility);
        }

        if (descriptionPanel != null)
            descriptionPanel.SetActive(false);

        if (descriptionClosePanel != null)
            descriptionClosePanel.SetActive(false);

        StartCoroutine(SetScrollPosition());
    }

    private IEnumerator SetScrollPosition()
    {
        yield return null;

        Canvas.ForceUpdateCanvases();
        scrollRect.StopMovement();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void OnClickDescriptionOpen(AbilitySlotUI slot)
    {
        if (slot == null || slot.Data == null)
            return;

        selectedSlot = slot;

        openUI.OnClickOpenUI();

        if (descriptionClosePanel != null) 
            descriptionClosePanel.SetActive(true);

        RefreshDescription();
    }

    public void OnClickBuyAbility()
    {
        if (selectedSlot == null ||
            selectedSlot.Data == null ||
            UserManager.Instance == null)
            return;

        if (UserManager.Instance.GetGold() < selectedSlot.Data.unlockCost[selectedSlot.Level])
            textFadeOut.AbilityLockMessageOpen(0);

        bool success = UserManager.Instance.TryBuyAbility(selectedSlot.Data, selectedSlot.Level);

        if (!success)
            return;

        selectedSlot.AbilityButtonUnlock();

        RefreshUI();

        OnClickDescriptionClose();
    }

    public void OnClickDescriptionClose()
    {
        closeUI.OnClickCloseUI();

        if (descriptionClosePanel != null)
            descriptionClosePanel.SetActive(false);

        selectedSlot = null;
    }

    private void RefreshDescription()
    {
        if (selectedSlot == null ||
            selectedSlot.Data == null ||
            UserManager.Instance == null)
            return;

        AbilityData data = selectedSlot.Data;
        int level = selectedSlot.Level;

        UserManager user = UserManager.Instance;

        bool isBought = user.IsAbilityBought(data.abilityType, level);

        if (nameText != null)
            nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.abilityName);

        if (descriptionText != null)
            descriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Text", data.abilityDescription, new object[]
            {
                data.bonusValue[level]
            });

        if (levelText != null)
            levelText.text = $"Lv. {level + 1}";

        if (costText != null)
            costText.text = isBought ? "" : "x " + data.unlockCost[level].ToString("N0");

        if (buyButton != null)
            buyButton.interactable = !isBought;
    }


    public void RefreshUI()
    {
        foreach (AbilitySlotUI slot in slots)
        {
            if (slot == null) 
                continue;

            slot.RefreshUI();
        }
    }


    private void OnDestroy()
    {
        if (buyButton != null)
            buyButton.onClick.RemoveListener(OnClickBuyAbility);
    }
}
