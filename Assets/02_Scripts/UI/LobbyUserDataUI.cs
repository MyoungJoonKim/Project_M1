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
                userExpSlider.value = Shared.userManager.GetUserExp();

            if (userIcon != null)
                userIcon.sprite = Shared.userManager.userData.userIcon;

            if (userNameText != null)
                userNameText.text = $"{Shared.userManager.userData.userName}";

            if (userLevelText != null)
                userLevelText.text = $"Lv. {Shared.userManager.GetUserLevel()}";

            yield return null;
        }
    }
}
