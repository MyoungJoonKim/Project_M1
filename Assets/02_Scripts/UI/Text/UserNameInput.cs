using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class UserNameInput : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panel;

    [Header("User Name TMP")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int maxLength = 10;
    [SerializeField] private int minLength = 2;


    private TMP_Text placeholderText;

    private void Start()
    {
        inputField.characterLimit = maxLength;

        placeholderText = inputField.placeholder as TMP_Text;

        placeholderText.text = placeholderText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "LOBBY_NICKNAME_PLACEHOLDER");

        Open();
    }

    private void Open()
    {
        if (UserManager.Instance.userData.userName.Length < minLength)
            panel.SetActive(true);
        else
            panel.SetActive(false);
    }

    public void OnClickApplyButton()
    {
        if (inputField.text.Length >= minLength)
        {
            UserManager.Instance.userData.userName = inputField.text;
            panel.SetActive(false);
        }
        else
        {
            if (placeholderText != null)
                placeholderText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "LOBBY_NICKNAME_PLACEHOLDER_WARNING");
        }
        SoundManager.Instance.PlayButtonClick();
    }



}
