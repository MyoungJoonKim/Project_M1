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
                userExpSlider.value = CoreService.userManager.GetUserExp();

            if (userIcon != null)
                userIcon.sprite = CoreService.userManager.userData.userIcon;

            if (userNameText != null)
                userNameText.text = $"{CoreService.userManager.userData.userName}";

            if (userLevelText != null)
                userLevelText.text = $"Lv. {CoreService.userManager.GetUserLevel()}";

            yield return null;
        }
    }
}
