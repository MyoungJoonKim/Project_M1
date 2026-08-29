using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUserDataUI : MonoBehaviour
{
    [Header("UserExp Slider")]
    [SerializeField] private Slider userExpSlider;

    [Header("User Icon")]
    [SerializeField] private Image userIcon;

    [Header("User Texts")]
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text userLevelText;

    private void Start()
    {
        StartCoroutine(userDataUpdate());
    }

    private IEnumerator userDataUpdate()
    {
        while(true)
        {
            if (userExpSlider != null) 
                userExpSlider.value = UserManager.Instance.GetUserExp();

            if (userIcon != null)
                userIcon.sprite = UserManager.Instance.userData.userIcon;

            if (userNameText != null)
                userNameText.text = $"{UserManager.Instance.userData.userName}";

            if (userLevelText != null)
                userLevelText.text = $"Lv. {UserManager.Instance.GetUserLevel()}";

            yield return null;
        }
    }
}
