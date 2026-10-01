using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggleUI : MonoBehaviour
{
    [Header("Lobby Menu Icons")]
    [SerializeField] private Toggle menuToggle;
    [SerializeField] private GameObject menuIcon;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject lockButton;
    [SerializeField] private TMP_Text text;

    [Header("Panel")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextFadeOut textFadeOut;

    [Header("Menu Lock Off")]
    [SerializeField] private int unLockLevel;

    [Header("Icon Scale")]
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float selectScale = 1.8f;
    [SerializeField] private float scaleSpeed = 5f;
    [SerializeField] private Vector3 selectPosition = new Vector3(0f, 0f, 0f);

    private Coroutine lockCheckCoroutine;
    private Coroutine iconScaleCoroutine;

    private Vector3 normalPosition;
    private bool isUnlock;

    public int UnLockLevel => unLockLevel;

    private void Start()
    {
        normalPosition = Vector3.zero;
        SetLockMenu();

        lockCheckCoroutine = StartCoroutine(UnlockCheckRoutine());
        iconScaleCoroutine = StartCoroutine(MenuIconAimationRoutine());
    }

    private void SetLockMenu()
    {
        if (menuToggle != null)
            menuToggle.interactable = false;
        
        if (lockIcon != null)
            lockIcon.SetActive(true);

        if (menuIcon != null)
            menuIcon.SetActive(false);

        if (text != null)
            text.gameObject.SetActive(false);

        if (menuPanel != null)
            menuPanel.SetActive(false);
        
        isUnlock = false;
    }
    private void SetLockedState()
    {
        if (menuToggle != null)
            menuToggle.interactable = true;

        if (lockIcon != null)
            lockIcon.SetActive(false);

        if (lockButton != null)
            lockButton.SetActive(false);

        if (menuIcon != null)
            menuIcon.SetActive(true);

        if (text != null)
            text.gameObject.SetActive(true);

        isUnlock = true;
    }

    private IEnumerator UnlockCheckRoutine()
    {
        while (!isUnlock)
        {
            if (UserManager.Instance.GetUserLevel() >= unLockLevel)
            {
                SetLockedState();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator MenuIconAimationRoutine()
    {
        while (true)
        {
            if (!isUnlock)
            {
                yield return null;
                continue;
            }

            float scale = menuToggle.isOn ? selectScale : normalScale;
            Vector3 position = menuToggle.isOn ? selectPosition : normalPosition;

            menuIcon.transform.localScale = Vector3.Lerp(menuIcon.transform.localScale, Vector3.one * scale, Time.unscaledDeltaTime * scaleSpeed);

            menuIcon.transform.localPosition = Vector3.Lerp(menuIcon.transform.localPosition, position, Time.unscaledDeltaTime * scaleSpeed);

            menuPanel.SetActive(menuToggle.isOn);

            yield return null;
        }
    }

    public void OnClickToggleButton()
    {
        if (UserManager.Instance.GetUserLevel() < unLockLevel)
        {
            textFadeOut.MenuLockMessageOpen(unLockLevel);
        }
        SoundManager.Instance.PlayButtonClick();
    }
}
