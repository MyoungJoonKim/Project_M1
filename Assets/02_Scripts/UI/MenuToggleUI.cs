using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggleUI : MonoBehaviour
{
    [Header("Lobby Menu Icons")]
    [SerializeField] private Toggle menuToggle;
    [SerializeField] private GameObject MenuIcon;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject lockButton;
    [SerializeField] private TMP_Text text;

    [Header("Panel")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextFadeOut textFadeOut;

    [Header("Menu Lock Off")]
    [SerializeField] private int lockOffLevel;

    [Header("Icon Scale")]
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float selectScale = 1.8f;
    [SerializeField] private float scaleSpeed = 5f;
    [SerializeField] private Vector3 selectPosition = new Vector3(0f, 0f, 0f);

    private Coroutine lockCheckCoroutine;
    private Coroutine iconScaleCoroutine;

    private Vector3 normalPosition;
    private bool isUnlock;

    public int LockOffLevel => lockOffLevel;

    private void Start()
    {
        normalPosition = Vector3.zero;
        SetLockMenu();

        lockCheckCoroutine = StartCoroutine(MenuLockUpdate());
        iconScaleCoroutine = StartCoroutine(MenuIconScaleUpdate());
    }

    private void SetLockMenu()
    {
        if (menuToggle != null)
            menuToggle.interactable = false;
        
        if (lockIcon != null)
            lockIcon.SetActive(true);

        if (MenuIcon != null)
            MenuIcon.SetActive(false);

        if (text != null)
            text.gameObject.SetActive(false);

        if (menuPanel != null)
            menuPanel.SetActive(false);
        
        isUnlock = false;
    }
    private void SetUnlockMenu()
    {
        if (menuToggle != null)
            menuToggle.interactable = true;

        if (lockIcon != null)
            lockIcon.SetActive(false);

        if (lockButton != null)
            lockButton.SetActive(false);

        if (MenuIcon != null)
            MenuIcon.SetActive(true);

        if (text != null)
            text.gameObject.SetActive(true);

        isUnlock = true;
    }

    private IEnumerator MenuLockUpdate()
    {
        while (!isUnlock)
        {
            if (UserManager.Instance.GetUserLevel() >= lockOffLevel)
            {
                SetUnlockMenu();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator MenuIconScaleUpdate()
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

            MenuIcon.transform.localScale = Vector3.Lerp(MenuIcon.transform.localScale, Vector3.one * scale, Time.unscaledDeltaTime * scaleSpeed);

            MenuIcon.transform.localPosition = Vector3.Lerp(MenuIcon.transform.localPosition, position, Time.unscaledDeltaTime * scaleSpeed);

            menuPanel.SetActive(menuToggle.isOn);

            yield return null;
        }
    }

    public void OnClickToggleButton()
    {
        if (UserManager.Instance.GetUserLevel() < lockOffLevel)
        {
            textFadeOut.Open(lockOffLevel);
        }
    }
}
