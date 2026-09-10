using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject giveUpConfirmUI;

    [Header("Toggle")]
    [SerializeField] private Toggle joystickToggle;
    [SerializeField] private Toggle damageTextToggle;

    [Header("Skill List UI")]
    [SerializeField] private SkillSlotUI[] activeSlots;
    [SerializeField] private SkillSlotUI[] passiveSlots;
    [SerializeField] private Transform skillRoot;

    [Header("Managers")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private PassiveSkillManager passiveSkillManager;

    private Coroutine skillListUICoroutine;

    private void Start()
    {
        SetOptionSetting();

        if (joystickToggle != null)
        {
            joystickToggle.onValueChanged.RemoveAllListeners();
            joystickToggle.onValueChanged.AddListener(OnClickJoystickVisible);
        }

        if (damageTextToggle != null)
        {
            damageTextToggle.onValueChanged.RemoveAllListeners();
            damageTextToggle.onValueChanged.AddListener(OnClickDamageTextVisible);
        }

        if (pauseUI != null)
            pauseUI.SetActive(false);

        if (optionUI != null)
            optionUI.SetActive(false);

        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(false);

        ClearAllSlots();
    }

    public void Open()
    {
        if (pauseUI != null)
            pauseUI.SetActive(true);

        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(false);

        Time.timeScale = 0f;

        if (skillListUICoroutine != null)
            StopCoroutine(skillListUICoroutine);

        skillListUICoroutine = StartCoroutine(SkillListUIUpdate());
    }

    private IEnumerator SkillListUIUpdate()
    {
        if (skillRoot == null)
        {
            ClearAllSlots();
            skillListUICoroutine = null;
            yield break;
        }
        PlayerSkillManager[] managers = skillRoot.GetComponentsInChildren<PlayerSkillManager>();

        for (int i = 0; i < activeSlots.Length; i++)
        {
            if (activeSlots[i] == null)
                continue;

            if (i < managers.Length)
            {
                PlayerSkillManager manager = managers[i];

                if (manager != null && manager.Data != null)
                {
                    activeSlots[i].SetSlot(manager.Data, manager.CurrentLevel);
                }
                else
                    activeSlots[i].ClearSlot();
            }
            else
                activeSlots[i].ClearSlot();

            yield return null;
        }

        Dictionary<PassiveSkillData, int> passiveSkills = passiveSkillManager.GetPassiveSkills();

        int passiveIndex = 0;

        foreach (var skill in passiveSkills)
        {
            if (passiveIndex >= passiveSlots.Length)
                break;

            passiveSlots[passiveIndex].SetSlot(skill.Key, skill.Value);
            passiveIndex++;
        }

        for (int i = passiveIndex; i < passiveSlots.Length; i++)
        {
            passiveSlots[i].ClearSlot();
        }

        skillListUICoroutine = null;
    }

    private void ClearAllSlots()
    {
        if (activeSlots != null)
        {
            for (int i = 0; i < activeSlots.Length; i++)
            {
                if (activeSlots[i] != null)
                    activeSlots[i].ClearSlot();
            }
        }

        if (passiveSlots != null)
        {
            for (int i = 0; i < passiveSlots.Length; i++)
            {
                if (passiveSlots[i] != null)
                    passiveSlots[i].ClearSlot();
            }
        }
    }

    
    private void SetOptionSetting()
    {
        if (UserManager.Instance != null)
        {
            bool joystickVisible = UserManager.Instance.isJoystickVisible;
            bool damageTextVisible = UserManager.Instance.isDamageTextVisible;

            if (joystickToggle != null)
                joystickToggle.SetIsOnWithoutNotify(joystickVisible);

            if (damageTextToggle != null)
                damageTextToggle.SetIsOnWithoutNotify(damageTextVisible);

            if (joystick !=  null)
                joystick.SetJoystickVisible(joystickVisible);

            if (DamageTextManager.Instance != null)
                DamageTextManager.Instance.SetDamageTextVisible(damageTextVisible);
        }
    }

    public void OnClickPlayButton()
    {
        if (skillListUICoroutine != null)
        {
            StopCoroutine(skillListUICoroutine);
            skillListUICoroutine = null;
        }

        pauseUI.SetActive(false);
        Time.timeScale = 1f;

        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickLobbyButton()
    {
        if (giveUpConfirmUI != null)
            giveUpConfirmUI.SetActive(true);

        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickGiveUpButton()
    {
        Time.timeScale = 1f;

        if (SceneLoadManager.Instance != null)
            SceneLoadManager.Instance.ChangeScene(SceneType.LOBBY, false);

        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickOptionButton()
    {
        if (optionUI != null)
            optionUI.SetActive(!optionUI.activeSelf);

        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickJoystickVisible(bool visible)
    {
        UserManager.Instance.SetJoystickVisible(visible);

        if (joystick != null)
            joystick.SetJoystickVisible(visible);

        SoundManager.Instance.PlayButtonClick();
    }

    public void OnClickDamageTextVisible(bool visible)
    {
        UserManager.Instance.SetDamageTextVisible(visible);

        if (DamageTextManager.Instance != null)
            DamageTextManager.Instance.SetDamageTextVisible(visible);

        SoundManager.Instance.PlayButtonClick();
    }
}
