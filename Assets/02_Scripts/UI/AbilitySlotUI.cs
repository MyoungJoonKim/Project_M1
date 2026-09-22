using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using DG.Tweening;

public class AbilitySlotUI : MonoBehaviour
{
    [Header("Ability Info")]
    [SerializeField] private AbilityData abilityData;
    [SerializeField] private int abilityLevel;

    [Header("Ability Info")]
    [SerializeField] private Image icon;
    [SerializeField] private Image iconPanel;
    [SerializeField] private Sprite lockIcon;
    
    [Header("Ability Button")]
    [SerializeField] private Button descriptionButton;

    public AbilityData Data => abilityData;
    public int Level => abilityLevel;

    private Color originalColor = Color.white;

    private AbilityManager abilityManager;
    

    public void Init(AbilityManager manager)
    {
        abilityManager = manager;

        if (descriptionButton != null)
        {
            descriptionButton.onClick.RemoveListener(OnClickDescription);
            descriptionButton.onClick.AddListener(OnClickDescription);
        }

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (descriptionButton != null)
            descriptionButton.onClick.RemoveListener(OnClickDescription);

        if (icon != null)
            icon.transform.DOKill();

        if (iconPanel != null)
            iconPanel.DOKill();
    }

    public void RefreshUI()
    {
        if (abilityData == null || UserManager.Instance == null)
            return;

        if (abilityLevel < 0 ||
            abilityLevel >= abilityData.unlockLevel.Length ||
            abilityLevel >= abilityData.unlockCost.Length ||
            abilityLevel >= abilityData.bonusValue.Length) 
            return;

        UserManager user = UserManager.Instance;

        bool isBought = user.IsAbilityBought(abilityData.abilityType, abilityLevel);

        bool isUnlocked = user.GetUserLevel() >= abilityData.unlockLevel[abilityLevel];

        if (icon != null)
        {
            icon.sprite = abilityData.icon;
            icon.color = isBought ? originalColor : Color.gray;
        }

        if (!isUnlocked)
            icon.sprite = lockIcon;

        if (descriptionButton != null)
            descriptionButton.interactable = isUnlocked;
    }


    public void OnClickDescription()
    {
        if (abilityManager == null || abilityData == null) 
            return;

        abilityManager.OnClickDescriptionOpen(this);
    }


    public Tween AbilityButtonUnlock(float delay = 0f)
    {
        if (icon == null || iconPanel == null)
            return null;

        icon.transform.DOKill();
        icon.DOKill();
        iconPanel.DOKill();

        icon.transform.localScale = Vector3.one;
        icon.color = Color.gray;

        Color panelColor = iconPanel.color;
        panelColor.a = 1f;
        iconPanel.color = panelColor;

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(delay);

        sequence.Append(icon.DOColor(Color.white, 0.15f));

        sequence.Join(icon.transform.DOScale(1.25f, 0.2f).SetEase(Ease.OutBack));

        sequence.Append(iconPanel.DOFade(0.35f, 0.08f));
        sequence.Append(iconPanel.DOFade(1f, 0.08f));
        sequence.Append(iconPanel.DOFade(0.5f, 0.08f));
        sequence.Append(iconPanel.DOFade(1f, 0.08f));

        sequence.Append(icon.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));

        sequence.SetUpdate(true);
        return sequence;
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        RefreshUI();
    }
}
