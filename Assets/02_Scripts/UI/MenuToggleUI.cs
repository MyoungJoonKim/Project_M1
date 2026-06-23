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
    [SerializeField] private TMP_Text text;

    [Header("Menu Lock Off")]
    [SerializeField] private int lockOffLevel;

    private Coroutine ToggleCoroutine;

    private void Start()
    {
        lockIcon.SetActive(true);
        MenuIcon.SetActive(false);
        text.gameObject.SetActive(false);

        if (ToggleCoroutine != null)
            StopCoroutine(ToggleCoroutine);

        ToggleCoroutine = StartCoroutine(MenuLockUpdate());
    }

    IEnumerator MenuLockUpdate()
    {
        if (lockIcon.activeSelf)
        {
            menuToggle.interactable = false;
        }

        while (!menuToggle.interactable)
        {
            if (Shared.userManager.GetUserLevel() >= lockOffLevel)
            {
                lockIcon.SetActive(false);
                MenuIcon.SetActive(true);
                menuToggle.interactable = true;
                text.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }

    IEnumerator MenuIconScaleUp() // 사용하도록 수정할것.
    {
        if (menuToggle.isOn)
        {
            Vector3 offset = new Vector3(0, -30, 0);
            MenuIcon.transform.localScale *= 1.8f;
            MenuIcon.transform.localPosition += offset;
        }
        else
        {
            MenuIcon.transform.localScale = new Vector3(1, 1, 1);
            MenuIcon.transform.localPosition = new Vector3(0, 0, 0);
        }
        yield return null;
    }
}
