using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class UserNameInput : MonoBehaviour
{
    [Header("User Name TMP")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int maxLength = 10;
    [SerializeField] private int minLength = 2;

    private OpenUI openUI;
    private CloseUI closeUI;

    private TMP_Text placeholderText;

    private void Start()
    {
        openUI = GetComponent<OpenUI>();
        closeUI = GetComponent<CloseUI>();

        inputField.characterLimit = maxLength;

        placeholderText = inputField.placeholder as TMP_Text;

        placeholderText.text = placeholderText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "LOBBY_NICKNAME_PLACEHOLDER");

        Open();
    }

    private void Open()
    {
        if (UserManager.Instance.userData.userName.Length < minLength)
            openUI.OnClickOpenUI();
    }

    public void OnClickApplyButton()
    {
        if (inputField.text.Length >= minLength)
        {
            UserManager.Instance.userData.userName = inputField.text;
            closeUI.OnClickCloseUI();
        }
        else
        {
            if (placeholderText != null)
                placeholderText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "LOBBY_NICKNAME_PLACEHOLDER_WARNING");
        }
    }



}
