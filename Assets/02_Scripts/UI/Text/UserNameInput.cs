using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserNameInput : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject panel;

    [Header("User Name TMP")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int maxLength = 10;
    [SerializeField] private int minLength = 2;


    private void Start()
    {
        inputField.characterLimit = maxLength;
        Open();
    }

    private void Open()
    {
        if (Shared.userManager.userData.userName.Length < minLength)
            panel.SetActive(true);
        else
            panel.SetActive(false);
    }
    public void OnClickApplyButton()
    {
        if (inputField.text.Length >= minLength)
        {
            Shared.userManager.userData.userName = inputField.text;
            panel.SetActive(false);
        }
        else
        {
            TMP_Text placeholderText = inputField.placeholder as TMP_Text;
            if (placeholderText != null)
                placeholderText.text = "두글자 이상 입력해주세요.";
        }
    }



}
